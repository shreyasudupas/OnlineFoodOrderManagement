namespace MenuOrder.Shared.EmailTemplate
{
    public static class WelcomeVendorTemplate
    {
        public static string WelocmeVendorEmail(string vendorName,string userName,string vendorRegisterUrl)
        {
            return @$"<!DOCTYPE html>
            <html>
            <body>

            <h4>Hello {userName},</h4>

            <p>Welcome to MenuOrder Vendor App. Your vendor name {vendorName} has been succesfully verified.  You can now login and verify,
            <a href={vendorRegisterUrl}'>Please click here</a>. Hopefully you will have a smooth and delighted experience using this application.</p>

            <h4><b>Regards,</b></h4><br/>
            Admin

<h5>-----------Please do not reply to this email since it is system generated mail.----------</h5>

            </body>
            </html>";
        }
    }
}
