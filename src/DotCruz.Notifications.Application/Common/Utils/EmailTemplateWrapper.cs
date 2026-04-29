namespace DotCruz.Notifications.Application.Common.Utils;

public static class EmailTemplateWrapper
{
    private const string Header = @"
    <!DOCTYPE html>
    <html>
    <head>
        <meta charset='UTF-8'>
        <style>
            body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
            .container { width: 100%; max-width: 600px; margin: 0 auto; border: 1px solid #eee; padding: 20px; }
            .header { border-bottom: 2px solid #0056b3; padding-bottom: 10px; margin-bottom: 20px; }
            .footer { border-top: 1px solid #eee; padding-top: 10px; margin-top: 20px; font-size: 12px; color: #777; }
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h2>DotCruz Notifications</h2>
            </div>
            <div class='content'>";

    private const string Footer = @"
            </div>
            <div class='footer'>
                <p>&copy; {0} DotCruz. Todos os direitos reservados.</p>
            </div>
        </div>
    </body>
    </html>";

    public static string Wrap(string content)
    {
        var year = DateTime.UtcNow.Year.ToString();
        var footerWithYear = string.Format(Footer, year);
        
        return $"{Header}{content}{footerWithYear}";
    }
}
