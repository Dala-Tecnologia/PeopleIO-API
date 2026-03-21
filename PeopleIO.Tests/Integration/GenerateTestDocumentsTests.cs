using Microsoft.Extensions.Configuration;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Xunit;

namespace PeopleIO.Tests.Integration;

/// <summary>
/// Gera os arquivos de documento para uso nos testes de integração.
///
/// Os dados gerados correspondem aos valores usados em DocumentValidationServiceIntegrationTests.
///
/// PRÉ-REQUISITO: crie appsettings.IntegrationTests.json (baseado no .example ao lado).
///
/// COMO RODAR:
///   dotnet test --filter "Category=GenerateTestDocs"
///
/// Os arquivos são salvos em TestDocuments/ (ou nos caminhos definidos no appsettings).
/// </summary>
[Trait("Category", "GenerateTestDocs")]
public class GenerateTestDocumentsTests
{
    private readonly IConfiguration _config;
    private readonly string _outputPath;

    // Estes valores devem coincidir com os usados em DocumentValidationServiceIntegrationTests
    private const string RgNumero = "1234567";
    private const string RgUf = "SP";
    private static readonly DateTime RgDataEmissao = new(2015, 6, 20);
    private const string RgNome = "CANDIDATO TESTE DA SILVA";

    private const string CnhNumero = "12345678900";
    private const string CnhCpfFormatado = "123.456.789-00";
    private static readonly DateTime CnhDataNascimento = new(1990, 5, 15);
    private static readonly DateTime CnhDataVencimento = new(2028, 5, 15);
    private const string CnhNome = "CANDIDATO TESTE DA SILVA";

    private const string CompNome = "João da Silva";
    private const string CompRua = "Rua das Flores";
    private const string CompCidade = "São Paulo";
    private const string CompCep = "01310100";

    public GenerateTestDocumentsTests()
    {
        _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.IntegrationTests.json", optional: false)
            .Build();

        _outputPath = Path.Combine(Directory.GetCurrentDirectory(), "TestDocuments");
        Directory.CreateDirectory(_outputPath);
    }

    [Fact]
    public void GerarTodosOsDocumentos()
    {
        GerarRG();
        GerarCNH();
        GerarComprovanteResidencia();
    }

    private void GerarRG()
    {
        var path = GetPath("RG", "rg_teste.pdf");

        using var document = new PdfDocument();
        var page = document.AddPage();
        page.Width = XUnit.FromCentimeter(14.8);
        page.Height = XUnit.FromCentimeter(10.5);

        using var gfx = XGraphics.FromPdfPage(page);

        var boldFont = new XFont("Arial", 10, XFontStyle.Bold);
        var regularFont = new XFont("Arial", 9, XFontStyle.Regular);
        var labelFont = new XFont("Arial", 7, XFontStyle.Regular);
        var bigFont = new XFont("Arial", 18, XFontStyle.Bold);

        // Fundo azul claro
        gfx.DrawRectangle(XBrushes.LightSteelBlue, 0, 0, page.Width, page.Height);

        // Header
        gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(26, 35, 126)), 0, 0, page.Width, 40);
        gfx.DrawString("REPÚBLICA FEDERATIVA DO BRASIL", new XFont("Arial", 10, XFontStyle.Bold),
            XBrushes.White, new XRect(0, 4, page.Width, 15), XStringFormats.TopCenter);
        gfx.DrawString($"ESTADO DE {RgUf} – SSP/{RgUf}", new XFont("Arial", 8, XFontStyle.Regular),
            XBrushes.White, new XRect(0, 18, page.Width, 15), XStringFormats.TopCenter);
        gfx.DrawString("CARTEIRA DE IDENTIDADE / REGISTRO GERAL", new XFont("Arial", 9, XFontStyle.Bold),
            XBrushes.White, new XRect(0, 28, page.Width, 12), XStringFormats.TopCenter);

        // Foto placeholder
        gfx.DrawRectangle(XPens.Gray, XBrushes.White, 20, 55, 80, 100);
        gfx.DrawString("FOTO", labelFont, XBrushes.Gray, new XRect(20, 55, 80, 100), XStringFormats.Center);

        double x = 115;
        double y = 55;

        // RG
        gfx.DrawString("REGISTRO GERAL", labelFont, XBrushes.DimGray, x, y + 10);
        gfx.DrawString(RgNumero, bigFont, new XSolidBrush(XColor.FromArgb(26, 35, 126)), x, y + 28);

        // Nome
        gfx.DrawString("NOME", labelFont, XBrushes.DimGray, x, y + 48);
        gfx.DrawString(RgNome, boldFont, XBrushes.Black, x, y + 62);

        // Data de Emissão / UF
        gfx.DrawString("DATA DE EMISSÃO", labelFont, XBrushes.DimGray, x, y + 80);
        gfx.DrawString(RgDataEmissao.ToString("dd/MM/yyyy"), regularFont, XBrushes.Black, x, y + 93);

        gfx.DrawString("ÓRGÃO EXPEDIDOR / UF", labelFont, XBrushes.DimGray, x + 120, y + 80);
        gfx.DrawString($"SSP/{RgUf}", regularFont, XBrushes.Black, x + 120, y + 93);

        // Naturalidade
        gfx.DrawString("NATURALIDADE", labelFont, XBrushes.DimGray, x, y + 110);
        gfx.DrawString(RgUf, regularFont, XBrushes.Black, x, y + 123);

        document.Save(path);
    }

    private void GerarCNH()
    {
        var path = GetPath("CNH", "cnh_teste.pdf");

        using var document = new PdfDocument();
        var page = document.AddPage();
        page.Width = XUnit.FromCentimeter(14.8);
        page.Height = XUnit.FromCentimeter(10.5);

        using var gfx = XGraphics.FromPdfPage(page);

        var boldFont = new XFont("Arial", 10, XFontStyle.Bold);
        var regularFont = new XFont("Arial", 9, XFontStyle.Regular);
        var labelFont = new XFont("Arial", 7, XFontStyle.Regular);
        var bigFont = new XFont("Arial", 14, XFontStyle.Bold);

        // Fundo amarelo claro
        gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(255, 248, 225)), 0, 0, page.Width, page.Height);

        // Header laranja
        gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(230, 81, 0)), 0, 0, page.Width, 40);
        gfx.DrawString("REPÚBLICA FEDERATIVA DO BRASIL", new XFont("Arial", 10, XFontStyle.Bold),
            XBrushes.White, new XRect(0, 4, page.Width, 15), XStringFormats.TopCenter);
        gfx.DrawString("CARTEIRA NACIONAL DE HABILITAÇÃO – DETRAN/SP", new XFont("Arial", 8, XFontStyle.Regular),
            XBrushes.White, new XRect(0, 18, page.Width, 15), XStringFormats.TopCenter);
        gfx.DrawString("CNH", new XFont("Arial", 9, XFontStyle.Bold),
            XBrushes.White, new XRect(0, 28, page.Width, 12), XStringFormats.TopCenter);

        // Foto placeholder
        gfx.DrawRectangle(XPens.Gray, XBrushes.White, 20, 55, 80, 100);
        gfx.DrawString("FOTO", labelFont, XBrushes.Gray, new XRect(20, 55, 80, 100), XStringFormats.Center);

        double x = 115;
        double y = 55;

        // Nº Registro
        gfx.DrawString("Nº REGISTRO", labelFont, XBrushes.DimGray, x, y + 10);
        gfx.DrawString(CnhNumero, bigFont, new XSolidBrush(XColor.FromArgb(230, 81, 0)), x, y + 26);

        // Nome
        gfx.DrawString("NOME", labelFont, XBrushes.DimGray, x, y + 44);
        gfx.DrawString(CnhNome, boldFont, XBrushes.Black, x, y + 58);

        // CPF
        gfx.DrawString("CPF", labelFont, XBrushes.DimGray, x, y + 74);
        gfx.DrawString(CnhCpfFormatado, regularFont, XBrushes.Black, x, y + 88);

        // Data Nascimento / Validade
        gfx.DrawString("DATA NASCIMENTO", labelFont, XBrushes.DimGray, x, y + 104);
        gfx.DrawString(CnhDataNascimento.ToString("dd/MM/yyyy"), regularFont, XBrushes.Black, x, y + 118);

        gfx.DrawString("VALIDADE", labelFont, XBrushes.DimGray, x + 130, y + 104);
        gfx.DrawString(CnhDataVencimento.ToString("dd/MM/yyyy"), regularFont, XBrushes.Black, x + 130, y + 118);

        document.Save(path);
    }

    private void GerarComprovanteResidencia()
    {
        var path = GetPath("ComprovanteResidencia", "comprovante_teste.pdf");

        using var document = new PdfDocument();
        var page = document.AddPage();

        using var gfx = XGraphics.FromPdfPage(page);

        var titleFont = new XFont("Arial", 16, XFontStyle.Bold);
        var subtitleFont = new XFont("Arial", 12, XFontStyle.Regular);
        var boldFont = new XFont("Arial", 11, XFontStyle.Bold);
        var labelFont = new XFont("Arial", 11, XFontStyle.Regular);
        var smallFont = new XFont("Arial", 8, XFontStyle.Regular);

        gfx.DrawRectangle(XBrushes.White, 0, 0, page.Width, page.Height);

        double margin = 60;
        double y = 70;

        // Cabeçalho
        gfx.DrawString("COMPANHIA DE ENERGIA ELÉTRICA", titleFont, XBrushes.Black,
            new XRect(0, y, page.Width, 25), XStringFormats.TopCenter);
        y += 30;
        gfx.DrawString("Conta de Energia Elétrica", subtitleFont, XBrushes.Black,
            new XRect(0, y, page.Width, 20), XStringFormats.TopCenter);
        y += 22;
        gfx.DrawString("Referência: Fevereiro/2026", new XFont("Arial", 10, XFontStyle.Regular),
            XBrushes.Gray, new XRect(0, y, page.Width, 15), XStringFormats.TopCenter);
        y += 35;

        // Linha divisória
        gfx.DrawLine(XPens.LightGray, margin, y, page.Width - margin, y);
        y += 15;

        // Dados do cliente
        gfx.DrawString("DADOS DO CLIENTE", boldFont, XBrushes.Black, margin, y);
        y += 22;

        DrawField(gfx, labelFont, boldFont, margin, ref y, "Nome:", CompNome);
        DrawField(gfx, labelFont, boldFont, margin, ref y, "Endereço:", CompRua);
        DrawField(gfx, labelFont, boldFont, margin, ref y, "Cidade/UF:", $"{CompCidade}/SP");
        DrawField(gfx, labelFont, boldFont, margin, ref y, "CEP:", CompCep);

        y += 10;
        gfx.DrawLine(XPens.LightGray, margin, y, page.Width - margin, y);
        y += 20;

        // Valores
        gfx.DrawString("VALORES DA FATURA", boldFont, XBrushes.Black, margin, y);
        y += 22;

        DrawValor(gfx, labelFont, margin, page.Width, ref y, "Energia consumida (kWh)", "R$ 150,00");
        DrawValor(gfx, labelFont, margin, page.Width, ref y, "Tributos e encargos", "R$ 45,00");

        y += 5;
        gfx.DrawLine(XPens.LightGray, margin, y, page.Width - margin, y);
        y += 12;

        DrawValor(gfx, boldFont, margin, page.Width, ref y, "TOTAL A PAGAR", "R$ 195,00");

        // Rodapé
        gfx.DrawString("Documento emitido para fins de comprovação de residência.",
            smallFont, XBrushes.Gray,
            new XRect(0, page.Height - 50, page.Width, 15), XStringFormats.TopCenter);

        document.Save(path);
    }

    private static void DrawField(XGraphics gfx, XFont labelFont, XFont boldFont,
        double x, ref double y, string label, string value)
    {
        gfx.DrawString(label, labelFont, XBrushes.Gray, x, y);
        gfx.DrawString(value, boldFont, XBrushes.Black, x + 90, y);
        y += 18;
    }

    private static void DrawValor(XGraphics gfx, XFont font, double margin, double pageWidth,
        ref double y, string descricao, string valor)
    {
        gfx.DrawString(descricao, font, XBrushes.Black, margin, y);
        gfx.DrawString(valor, font, XBrushes.Black,
            new XRect(0, y, pageWidth - margin, 15), XStringFormats.TopRight);
        y += 18;
    }

    private string GetPath(string key, string defaultFileName)
    {
        var configPath = _config[$"DocumentosParaTeste:{key}"];
        if (!string.IsNullOrWhiteSpace(configPath))
        {
            return Path.IsPathRooted(configPath)
                ? configPath
                : Path.Combine(Directory.GetCurrentDirectory(), configPath);
        }
        return Path.Combine(_outputPath, defaultFileName);
    }
}