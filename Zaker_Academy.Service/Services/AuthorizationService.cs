using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.Service.DTO_s;
using Zaker_Academy.Service.ErrorHandling;
using Zaker_Academy.Service.Helper;
using Zaker_Academy.Service.Interfaces;

using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Zaker_Academy.Service.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<applicationUser> userManager;
        private readonly IEmailService emailService;
        private readonly JwtHelper jwt;
        private readonly EmailHelper emailHelper;

        public AuthorizationService(UserManager<applicationUser> userManager, IOptions<JwtHelper> jwt, IEmailService emailService, IOptions<EmailHelper> emailHelper)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.jwt = jwt.Value;
            this.emailHelper = emailHelper.Value;
        }

        public async Task<ServiceResult> ConfirmResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var result = new ServiceResult();

            var user = await userManager.FindByEmailAsync(resetPasswordDto.UserEmail);
            if (user == null)
            {
                result.Message = "Password Reset Failed";
                result.Details = "User not found";
                return result;
            }

            var resetResult = await userManager.ResetPasswordAsync(user, Uri.UnescapeDataString(resetPasswordDto.Token), resetPasswordDto.NewPassword);
            if (!resetResult.Succeeded)
            {
                result.Message = "Password Reset Failed";
                result.Details = string.Join(", ", resetResult.Errors.Select(e => e.Description));
                return result;
            }

            result.succeeded = true;
            result.Message = "Password Reset Successful";
            return result;
        }

        public async Task<ServiceResult> CreateEmailTokenAsync(string username)
        {
            var res = new ServiceResult();
            var user = await userManager.FindByNameAsync(username);
            if (user is null)
            {
                res.Message = "Registration Field";
                res.Details = "internal Server Error";
                return res;
            }
            var emailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            res.succeeded = true;
            res.Details = emailToken;

            return res;
        }

        public async Task<ServiceResult> CreatePasswordTokenAsync(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user is null)
                return new ServiceResult()
                {
                    Details = "The Email address doesn't exist"
                    ,
                    Message = "Password Reset failed"
                };
            var Token = await userManager.GeneratePasswordResetTokenAsync(user);

            return new ServiceResult() { succeeded = true, Message = "Create Token Succeeded", Details = Token };
        }

        public async Task<ServiceResult> CreateTokenAsync(string username)
        {
            var res = new ServiceResult();
            var user = await userManager.FindByNameAsync(username);
            var roles = await userManager.GetRolesAsync(user!);
            if (user == null)
            {
                res.Message = "Registration Field";
                res.Details = "internal Server Error";
                return res;
            }
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.UserName!));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            var symKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signingCredentials = new SigningCredentials(symKey, SecurityAlgorithms.HmacSha256Signature);
            var SecurityToken = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwt.DurationInHours),
                signingCredentials: signingCredentials
                );
            res.succeeded = true;
            res.Details = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
            return res;
        }

        public async Task<ServiceResult> SendResetPasswordAsync(string UserEmail, string callBackUrl)
        {
            var result = new ServiceResult();

            var user = await userManager.FindByEmailAsync(UserEmail);
            if (user == null)
            {
                result.Message = "Password Reset Failed";
                result.Details = "User not found";
                return result;
            }

            try
            {
                // Read the HTML template content from the file
                string body = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n  <head>\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />\n    <meta name=\"x-apple-disable-message-reformatting\" />\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\n    <meta name=\"color-scheme\" content=\"light dark\" />\n    <meta name=\"supported-color-schemes\" content=\"light dark\" />\n    <title></title>\n    <style type=\"text/css\" rel=\"stylesheet\" media=\"all\">\n    /* Base ------------------------------ */\n    \n    @import url(\"https://fonts.googleapis.com/css?family=Nunito+Sans:400,700&display=swap\");\n    body {\n      width: 100% !important;\n      height: 100%;\n      margin: 0;\n      -webkit-text-size-adjust: none;\n    }\n    \n    a {\n      color: #3869D4;\n    }\n    \n    a img {\n      border: none;\n    }\n    \n    td {\n      word-break: break-word;\n    }\n    \n    .preheader {\n      display: none !important;\n      visibility: hidden;\n      mso-hide: all;\n      font-size: 1px;\n      line-height: 1px;\n      max-height: 0;\n      max-width: 0;\n      opacity: 0;\n      overflow: hidden;\n    }\n    /* Type ------------------------------ */\n    \n    body,\n    td,\n    th {\n      font-family: \"Nunito Sans\", Helvetica, Arial, sans-serif;\n    }\n    \n    h1 {\n      margin-top: 0;\n      color: #333333;\n      font-size: 22px;\n      font-weight: bold;\n      text-align: left;\n    }\n    \n    h2 {\n      margin-top: 0;\n      color: #333333;\n      font-size: 16px;\n      font-weight: bold;\n      text-align: left;\n    }\n    \n    h3 {\n      margin-top: 0;\n      color: #333333;\n      font-size: 14px;\n      font-weight: bold;\n      text-align: left;\n    }\n    \n    td,\n    th {\n      font-size: 16px;\n    }\n    \n    p,\n    ul,\n    ol,\n    blockquote {\n      margin: .4em 0 1.1875em;\n      font-size: 16px;\n      line-height: 1.625;\n    }\n    \n    p.sub {\n      font-size: 13px;\n    }\n    /* Utilities ------------------------------ */\n    \n    .align-right {\n      text-align: right;\n    }\n    \n    .align-left {\n      text-align: left;\n    }\n    \n    .align-center {\n      text-align: center;\n    }\n    \n    .u-margin-bottom-none {\n      margin-bottom: 0;\n    }\n    /* Buttons ------------------------------ */\n    \n    .button {\n      background-color: #3869D4;\n      border-top: 10px solid #3869D4;\n      border-right: 18px solid #3869D4;\n      border-bottom: 10px solid #3869D4;\n      border-left: 18px solid #3869D4;\n      display: inline-block;\n      color: #FFF;\n      text-decoration: none;\n      border-radius: 3px;\n      box-shadow: 0 2px 3px rgba(0, 0, 0, 0.16);\n      -webkit-text-size-adjust: none;\n      box-sizing: border-box;\n    }\n    \n    .button--green {\n      background-color: #22BC66;\n      border-top: 10px solid #22BC66;\n      border-right: 18px solid #22BC66;\n      border-bottom: 10px solid #22BC66;\n      border-left: 18px solid #22BC66;\n    }\n    \n    .button--red {\n      background-color: #FF6136;\n      border-top: 10px solid #FF6136;\n      border-right: 18px solid #FF6136;\n      border-bottom: 10px solid #FF6136;\n      border-left: 18px solid #FF6136;\n    }\n    \n    @media only screen and (max-width: 500px) {\n      .button {\n        width: 100% !important;\n        text-align: center !important;\n      }\n    }\n    /* Attribute list ------------------------------ */\n    \n    .attributes {\n      margin: 0 0 21px;\n    }\n    \n    .attributes_content {\n      background-color: #F4F4F7;\n      padding: 16px;\n    }\n    \n    .attributes_item {\n      padding: 0;\n    }\n    /* Related Items ------------------------------ */\n    \n    .related {\n      width: 100%;\n      margin: 0;\n      padding: 25px 0 0 0;\n      -premailer-width: 100%;\n      -premailer-cellpadding: 0;\n      -premailer-cellspacing: 0;\n    }\n    \n    .related_item {\n      padding: 10px 0;\n      color: #CBCCCF;\n      font-size: 15px;\n      line-height: 18px;\n    }\n    \n    .related_item-title {\n      display: block;\n      margin: .5em 0 0;\n    }\n    \n    .related_item-thumb {\n      display: block;\n      padding-bottom: 10px;\n    }\n    \n    .related_heading {\n      border-top: 1px solid #CBCCCF;\n      text-align: center;\n      padding: 25px 0 10px;\n    }\n    /* Discount Code ------------------------------ */\n    \n    .discount {\n      width: 100%;\n      margin: 0;\n      padding: 24px;\n      -premailer-width: 100%;\n      -premailer-cellpadding: 0;\n      -premailer-cellspacing: 0;\n      background-color: #F4F4F7;\n      border: 2px dashed #CBCCCF;\n    }\n    \n    .discount_heading {\n      text-align: center;\n    }\n    \n    .discount_body {\n      text-align: center;\n      font-size: 15px;\n    }\n    /* Social Icons ------------------------------ */\n    \n    .social {\n      width: auto;\n    }\n    \n    .social td {\n      padding: 0;\n      width: auto;\n    }\n    \n    .social_icon {\n      height: 20px;\n      margin: 0 8px 10px 8px;\n      padding: 0;\n    }\n    /* Data table ------------------------------ */\n    \n    .purchase {\n      width: 100%;\n      margin: 0;\n      padding: 35px 0;\n      -premailer-width: 100%;\n      -premailer-cellpadding: 0;\n      -premailer-cellspacing: 0;\n    }\n    \n    .purchase_content {\n      width: 100%;\n      margin: 0;\n      padding: 25px 0 0 0;\n      -premailer-width: 100%;\n      -premailer-cellpadding: 0;\n      -premailer-cellspacing: 0;\n    }\n    \n    .purchase_item {\n      padding: 10px 0;\n      color: #51545E;\n      font-size: 15px;\n      line-height: 18px;\n    }\n    \n    .purchase_heading {\n      padding-bottom: 8px;\n      border-bottom: 1px solid #EAEAEC;\n    }\n    \n    .purchase_heading p {\n      margin: 0;\n      color: #85878E;\n      font-size: 12px;\n    }\n    \n    .purchase_footer {\n      padding-top: 15px;\n      border-top: 1px solid #EAEAEC;\n    }\n    \n    .purchase_total {\n      margin: 0;\n      text-align: right;\n      font-weight: bold;\n      color: #333333;\n    }\n    \n    .purchase_total--label {\n      padding: 0 15px 0 0;\n    }\n    \n    body {\n      background-color: #F2F4F6;\n      color: #51545E;\n    }\n    \n    p {\n      color: #51545E;\n    }\n    \n    .email-wrapper {\n      width: 100%;\n      margin: 0;\n      padding: 0;\n      -premailer-width: 100%;\n      -premailer-cellpadding: 0;\n      -premailer-cellspacing: 0;\n      background-color: #F2F4F6;\n    }\n    \n    .email-content {\n      width: 100%;\n      margin: 0;\n      padding: 0;\n      -premailer-width: 100%;\n      -premailer-cellpadding: 0;\n      -premailer-cellspacing: 0;\n    }\n    /* Masthead ----------------------- */\n    \n    .email-masthead {\n      padding: 25px 0;\n      text-align: center;\n    }\n    \n    .email-masthead_logo {\n      width: 94px;\n    }\n    \n    .email-masthead_name {\n      font-size: 16px;\n      font-weight: bold;\n      color: #A8AAAF;\n      text-decoration: none;\n      text-shadow: 0 1px 0 white;\n    }\n    /* Body ------------------------------ */\n    \n    .email-body {\n      width: 100%;\n      margin: 0;\n      padding: 0;\n      -premailer-width: 100%;\n      -premailer-cellpadding: 0;\n      -premailer-cellspacing: 0;\n    }\n    \n    .email-body_inner {\n      width: 570px;\n      margin: 0 auto;\n      padding: 0;\n      -premailer-width: 570px;\n      -premailer-cellpadding: 0;\n      -premailer-cellspacing: 0;\n      background-color: #FFFFFF;\n    }\n    \n    .email-footer {\n      width: 570px;\n      margin: 0 auto;\n      padding: 0;\n      -premailer-width: 570px;\n      -premailer-cellpadding: 0;\n      -premailer-cellspacing: 0;\n      text-align: center;\n    }\n    \n    .email-footer p {\n      color: #A8AAAF;\n    }\n    \n    .body-action {\n      width: 100%;\n      margin: 30px auto;\n      padding: 0;\n      -premailer-width: 100%;\n      -premailer-cellpadding: 0;\n      -premailer-cellspacing: 0;\n      text-align: center;\n    }\n    \n    .body-sub {\n      margin-top: 25px;\n      padding-top: 25px;\n      border-top: 1px solid #EAEAEC;\n    }\n    \n    .content-cell {\n      padding: 45px;\n    }\n    /*Media Queries ------------------------------ */\n    \n    @media only screen and (max-width: 600px) {\n      .email-body_inner,\n      .email-footer {\n        width: 100% !important;\n      }\n    }\n    \n    @media (prefers-color-scheme: dark) {\n      body,\n      .email-body,\n      .email-body_inner,\n      .email-content,\n      .email-wrapper,\n      .email-masthead,\n      .email-footer {\n        background-color: #333333 !important;\n        color: #FFF !important;\n      }\n      p,\n      ul,\n      ol,\n      blockquote,\n      h1,\n      h2,\n      h3,\n      span,\n      .purchase_item {\n        color: #FFF !important;\n      }\n      .attributes_content,\n      .discount {\n        background-color: #222 !important;\n      }\n      .email-masthead_name {\n        text-shadow: none !important;\n      }\n    }\n    \n    :root {\n      color-scheme: light dark;\n      supported-color-schemes: light dark;\n    }\n    </style>\n    <!--[if mso]>\n    <style type=\"text/css\">\n      .f-fallback  {\n        font-family: Arial, sans-serif;\n      }\n    </style>\n  <![endif]-->\n  </head>\n  <body>\n    <span class=\"preheader\">Use this link to reset your password. The link is only valid for 5 hours.</span>\n    <table class=\"email-wrapper\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\n      <tr>\n        <td align=\"center\">\n          <table class=\"email-content\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\n            <tr>\n              <td class=\"email-masthead\">\n                <a href=\"https://example.com\" class=\"f-fallback email-masthead_name\">\n               ZakerAcademy\n              </a>\n              </td>\n            </tr>\n            <!-- Email Body -->\n            <tr>\n              <td class=\"email-body\" width=\"570\" cellpadding=\"0\" cellspacing=\"0\">\n                <table class=\"email-body_inner\" align=\"center\" width=\"570\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\n                  <!-- Body content -->\n                  <tr>\n                    <td class=\"content-cell\">\n                      <div class=\"f-fallback\">\n                        <h1>Hi {name},</h1>\n                        <p>You recently requested to reset your password for your ZakerAcademy account. Use the Code below to reset it. <strong>This password reset is only valid for the next 5 hours.</strong></p>\n                        <!-- Action -->\n                        <table class=\"body-action\" align=\"center\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\n                          <tr>\n                            <td align=\"center\">\n                              <!-- Border based button\n           https://litmus.com/blog/a-guide-to-bulletproof-buttons-in-email-design -->\n                              <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" role=\"presentation\">\n                                <tr>\n                                  <td align=\"center\">\n                                    <p  class=\"f-fallback button--red\" target=\"_blank\">{Token}</p>\n                                  </td>\n                                </tr>\n                              </table>\n                            </td>\n                          </tr>\n                        </table>\n                        <p>For security, this request was received from a {{operating_system}} device using {{browser_name}}. If you did not request a password reset, please ignore this email or <a href=\"{{support_url}}\">contact support</a> if you have questions.</p>\n                        <p>Thanks,\n                          <br>The ZakerAcademy team</p>\n                        <!-- Sub copy -->\n                        \n                      </div>\n                    </td>\n                  </tr>\n                </table>\n              </td>\n            </tr>\n            <tr>\n              <td>\n                <table class=\"email-footer\" align=\"center\" width=\"570\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\n                  <tr>\n                    <td class=\"content-cell\" align=\"center\">\n                      <p class=\"f-fallback sub align-center\">\n                        [Company Name, LLC]\n                        <br>1234 Street Rd.\n                        <br>Suite 1234\n                      </p>\n                    </td>\n                  </tr>\n                </table>\n              </td>\n            </tr>\n          </table>\n        </td>\n      </tr>\n    </table>\n  </body>\n</html>"; var token = await userManager.GeneratePasswordResetTokenAsync(user);

                // Send reset password email
                body = body.Replace("{Token}", Uri.EscapeDataString(token));
                body = body.Replace("{name}", user.UserName);

                result = await emailService.sendAsync("Password Reset", body, emailHelper.Address, UserEmail);

                if (!result.succeeded)
                {
                    result.Message = "Password Reset Failed";
                    result.Details = "Error sending reset email";
                    return result;
                }

                result.succeeded = true;
                result.Message = "Password Reset Email Sent Successfully";
                return result;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, permission issues)
                // Log the exception or take appropriate action based on your requirements
                Console.WriteLine($"Error reading HTML template file: {ex.Message}");
                return new ServiceResult { Message = "Password Reset Failed", Details = "Internal Server Error" }; // or throw an exception if needed
            }
        }

        public async Task<ServiceResult> SendVerificationEmailAsync(string UserEmail, string callBackUrl)
        {
            var body = $"<!DOCTYPE html>\r\n<html>\r\n<head>\r\n\r\n  <meta charset=\"utf-8\">\r\n  <meta http-equiv=\"x-ua-compatible\" content=\"ie=edge\">\r\n  <title>Email Confirmation</title>\r\n  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n  <style type=\"text/css\">\r\n  /**\r\n   * Google webfonts. Recommended to include the .woff version for cross-client compatibility.\r\n   */\r\n  @media screen {{\r\n    @font-face {{\r\n      font-family: 'Source Sans Pro';\r\n      font-style: normal;\r\n      font-weight: 400;\r\n      src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');\r\n    }}\r\n    @font-face {{\r\n      font-family: 'Source Sans Pro';\r\n      font-style: normal;\r\n      font-weight: 700;\r\n      src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');\r\n    }}\r\n  }}\r\n  /**\r\n   * Avoid browser level font resizing.\r\n   * 1. Windows Mobile\r\n   * 2. iOS / OSX\r\n   */\r\n  body,\r\n  table,\r\n  td,\r\n  a {{\r\n    -ms-text-size-adjust: 100%; /* 1 */\r\n    -webkit-text-size-adjust: 100%; /* 2 */\r\n  }}\r\n  /**\r\n   * Remove extra space added to tables and cells in Outlook.\r\n   */\r\n  table,\r\n  td {{\r\n    mso-table-rspace: 0pt;\r\n    mso-table-lspace: 0pt;\r\n  }}\r\n  /**\r\n   * Better fluid images in Internet Explorer.\r\n   */\r\n  img {{\r\n    -ms-interpolation-mode: bicubic;\r\n  }}\r\n  /**\r\n   * Remove blue links for iOS devices.\r\n   */\r\n  a[x-apple-data-detectors] {{\r\n    font-family: inherit !important;\r\n    font-size: inherit !important;\r\n    font-weight: inherit !important;\r\n    line-height: inherit !important;\r\n    color: inherit !important;\r\n    text-decoration: none !important;\r\n  }}\r\n  /**\r\n   * Fix centering issues in Android 4.4.\r\n   */\r\n  div[style*=\"margin: 16px 0;\"] {{\r\n    margin: 0 !important;\r\n  }}\r\n  body {{\r\n    width: 100% !important;\r\n    height: 100% !important;\r\n    padding: 0 !important;\r\n    margin: 0 !important;\r\n  }}\r\n  /**\r\n   * Collapse table borders to avoid space between cells.\r\n   */\r\n  table {{\r\n    border-collapse: collapse !important;\r\n  }}\r\n  a {{\r\n    color: #1a82e2;\r\n  }}\r\n  img {{\r\n    height: auto;\r\n    line-height: 100%;\r\n    text-decoration: none;\r\n    border: 0;\r\n    outline: none;\r\n  }}\r\n  </style>\r\n\r\n</head>\r\n<body style=\"background-color: #e9ecef;\">\r\n\r\n  <!-- start preheader -->\r\n  <div class=\"preheader\" style=\"display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;\">\r\n    A preheader is the short summary text that follows the subject line when an email is viewed in the inbox.\r\n  </div>\r\n  <!-- end preheader -->\r\n\r\n  <!-- start body -->\r\n  <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n\r\n    <!-- start logo -->\r\n    <tr>\r\n      <td align=\"center\" bgcolor=\"#e9ecef\">\r\n        <!--[if (gte mso 9)|(IE)]>\r\n        <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">\r\n        <tr>\r\n        <td align=\"center\" valign=\"top\" width=\"600\">\r\n        <![endif]-->\r\n        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n          <tr>\r\n            <td align=\"center\" valign=\"top\" style=\"padding: 36px 24px;\">\r\n              <a href=\"https://www.blogdesire.com\" target=\"_blank\" style=\"display: inline-block;\">\r\n                <img src=\"https://www.blogdesire.com/wp-content/uploads/2019/07/blogdesire-1.png\" alt=\"Logo\" border=\"0\" width=\"48\" style=\"display: block; width: 48px; max-width: 48px; min-width: 48px;\">\r\n              </a>\r\n            </td>\r\n          </tr>\r\n        </table>\r\n        <!--[if (gte mso 9)|(IE)]>\r\n        </td>\r\n        </tr>\r\n        </table>\r\n        <![endif]-->\r\n      </td>\r\n    </tr>\r\n    <!-- end logo -->\r\n\r\n    <!-- start hero -->\r\n    <tr>\r\n      <td align=\"center\" bgcolor=\"#e9ecef\">\r\n        <!--[if (gte mso 9)|(IE)]>\r\n        <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">\r\n        <tr>\r\n        <td align=\"center\" valign=\"top\" width=\"600\">\r\n        <![endif]-->\r\n        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n          <tr>\r\n            <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;\">\r\n              <h1 style=\"margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;\">Confirm Your Email Address</h1>\r\n            </td>\r\n          </tr>\r\n        </table>\r\n        <!--[if (gte mso 9)|(IE)]>\r\n        </td>\r\n        </tr>\r\n        </table>\r\n        <![endif]-->\r\n      </td>\r\n    </tr>\r\n    <!-- end hero -->\r\n\r\n    <!-- start copy block -->\r\n    <tr>\r\n      <td align=\"center\" bgcolor=\"#e9ecef\">\r\n        <!--[if (gte mso 9)|(IE)]>\r\n        <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">\r\n        <tr>\r\n        <td align=\"center\" valign=\"top\" width=\"600\">\r\n        <![endif]-->\r\n        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n\r\n          <!-- start copy -->\r\n          <tr>\r\n            <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">\r\n              <p style=\"margin: 0;\">Tap the button below to confirm your email address. If you didn't create an account with <a href=\"https://blogdesire.com\">Paste</a>, you can safely delete this email.</p>\r\n            </td>\r\n          </tr>\r\n          <!-- end copy -->\r\n\r\n          <!-- start button -->\r\n          <tr>\r\n            <td align=\"left\" bgcolor=\"#ffffff\">\r\n              <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                <tr>\r\n                  <td align=\"center\" bgcolor=\"#ffffff\" style=\"padding: 12px;\">\r\n                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n                      <tr>\r\n                        <td align=\"center\" bgcolor=\"#1a82e2\" style=\"border-radius: 6px;\">\r\n                          <a href=\"{callBackUrl}\" target=\"_blank\" style=\"display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;\">Click Here</a>\r\n                        </td>\r\n                      </tr>\r\n                    </table>\r\n                  </td>\r\n                </tr>\r\n              </table>\r\n            </td>\r\n          </tr>\r\n          <!-- end button -->\r\n\r\n          <!-- start copy -->\r\n          <tr>\r\n            <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">\r\n              <p style=\"margin: 0;\">If that doesn't work, copy and paste the following link in your browser:</p>\r\n              <p style=\"margin: 0;\"><a href=\"{callBackUrl}\" target=\"_blank\">https://blogdesire.com/xxx-xxx-xxxx</a></p>\r\n            </td>\r\n          </tr>\r\n          <!-- end copy -->\r\n\r\n          <!-- start copy -->\r\n          <tr>\r\n            <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf\">\r\n              <p style=\"margin: 0;\">Cheers,<br> Paste</p>\r\n            </td>\r\n          </tr>\r\n          <!-- end copy -->\r\n\r\n        </table>\r\n        <!--[if (gte mso 9)|(IE)]>\r\n        </td>\r\n        </tr>\r\n        </table>\r\n        <![endif]-->\r\n      </td>\r\n    </tr>\r\n    <!-- end copy block -->\r\n\r\n    <!-- start footer -->\r\n    <tr>\r\n      <td align=\"center\" bgcolor=\"#e9ecef\" style=\"padding: 24px;\">\r\n        <!--[if (gte mso 9)|(IE)]>\r\n        <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">\r\n        <tr>\r\n        <td align=\"center\" valign=\"top\" width=\"600\">\r\n        <![endif]-->\r\n        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n\r\n          <!-- start permission -->\r\n          <tr>\r\n            <td align=\"center\" bgcolor=\"#e9ecef\" style=\"padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;\">\r\n              <p style=\"margin: 0;\">You received this email because we received a request for Registration for your account. If you didn't request Registration you can safely delete this email.</p>\r\n            </td>\r\n          </tr>\r\n          <!-- end permission -->\r\n\r\n          <!-- start unsubscribe -->\r\n          <tr>\r\n            <td align=\"center\" bgcolor=\"#e9ecef\" style=\"padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;\">\r\n              <p style=\"margin: 0;\">To stop receiving these emails, you can <a href=\"https://www.blogdesire.com\" target=\"_blank\">unsubscribe</a> at any time.</p>\r\n              <p style=\"margin: 0;\">Paste 1234 S. Broadway St. City, State 12345</p>\r\n            </td>\r\n          </tr>\r\n          <!-- end unsubscribe -->\r\n\r\n        </table>\r\n        <!--[if (gte mso 9)|(IE)]>\r\n        </td>\r\n        </tr>\r\n        </table>\r\n        <![endif]-->\r\n      </td>\r\n    </tr>\r\n    <!-- end footer -->\r\n\r\n  </table>\r\n  <!-- end body -->\r\n\r\n</body>\r\n</html>";
            var res = await emailService.sendAsync("Confirmation Email", body, emailHelper.Address, UserEmail);

            return res;
        }

        public async Task<ServiceResult> VerifyEmailAsync(string Email, string code)
        {
            var result = new ServiceResult();
            var user = await userManager.FindByEmailAsync(Email);
            if (user is null)
            {
                result.Message = "Verification Field";
                result.Details = "Invalid Email";
                return result;
            }
            var res = await userManager.ConfirmEmailAsync(user, code);
            if (!res.Succeeded)
            {
                result.Message = "Verification Field";
                result.Details = "Your Email Is Not confirmed , Please try again later and check your email";
                return result;
            }
            result.succeeded = true;
            return result;
        }
    }
}