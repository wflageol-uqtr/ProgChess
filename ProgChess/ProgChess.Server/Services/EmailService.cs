using SendGrid;
using SendGrid.Helpers.Mail;

namespace ProgChess.Server.Services;

public class EmailService(IConfiguration configuration): IEmailService
{
    public async Task<bool> SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            var apiKey = configuration.GetValue<string>("Sendgrid:ApiKey");
            var client = new SendGridClient(apiKey);
            var sendGridMessage = MailHelper.CreateSingleEmail(
                new EmailAddress("mathylefebvre@hotmail.com"), new EmailAddress("mathylefebvre@hotmail.com"), subject, message, BuildResetPasswordEmailHtml(message));
            var response = await client.SendEmailAsync(sendGridMessage);
            return response.StatusCode == System.Net.HttpStatusCode.OK ||
                   response.StatusCode == System.Net.HttpStatusCode.Accepted;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    private string BuildResetPasswordEmailHtml(string resetLink)
    {
        return $@"
            <div style='font-family: Arial, sans-serif; max-width: 500px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 24px; box-shadow: 0 4px 10px rgba(0,0,0,0.05);'>
            <h2 style='color: #00c950; text-align: center;'>ProgChess</h2>
            <p>Bonjour,</p>
            <p>Nous avons reçu une demande de réinitialisation de votre mot de passe.</p>
            <p>Pour réinitialiser votre mot de passe, cliquez sur le bouton ci-dessous :</p>
            <div style='text-align: center; margin-top: 24px;'>
                <a href='{resetLink}' style='display: inline-block; padding: 12px 20px; background-color: #00c950; color: white; text-decoration: none; border-radius: 6px; font-weight: bold;'>Réinitialiser le mot de passe</a>
            </div>
            <p style='font-size: 12px; color: #999; text-align: center; margin-top: 32px;'>&copy; {DateTime.Now.Year} ProgChess</p>
        </div>";
    }

}