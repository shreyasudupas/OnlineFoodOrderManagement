using System;
using System.Web;

namespace MenuOrder.Shared.EmailTemplate
{
    public static class RegisterVendorTemplate
    {
        public static string RegisterVendorEmail(string vendorName,string vendorEmail,string vendorRegisterUrl,string vendorId)
        {
            return $@"<!DOCTYPE html>
            <html>
            <body>

            <h4>Hello {vendorEmail.Split('@')[0]},</h4>

            <p>You have been reffered by this vendor {vendorName}. So Please register to this link <a href='{vendorRegisterUrl}?VendorId={vendorId}
&&VendorName={HttpUtility.UrlEncode(vendorName)}&&Email={HttpUtility.UrlEncode(vendorEmail)}'>Click Here</a> to enjoy all the benefits
provided by the MenuOrder Vendor App</p>

            <h4><b>Regards,</b></h4><br/>
            Admin

<h5>-----------Please do not reply to this email since it is system generated mail.----------</h5>

            </body>
            </html>";
        }
    }
}
