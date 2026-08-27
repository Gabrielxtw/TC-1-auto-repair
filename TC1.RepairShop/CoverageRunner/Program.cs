using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string rootDir = Directory.GetCurrentDirectory();
            string reportDir = Path.Combine(rootDir, "CoverageReport");

            Console.WriteLine("=== Iniciando Automação de Cobertura de Código ===");

            // 1. Limpa relatórios anteriores
            LimparDiretorios(reportDir);

            // 2. Garante que o ReportGenerator está instalado
            ExecutarComando("dotnet", "tool install -g dotnet-reportgenerator-globaltool");

            // 3. Executa os testes em todos os projetos exportando no formato cobertura
            Console.WriteLine("\n--> Executando testes...");
            ExecutarComando("dotnet", "test --collect:\"XPlat Code Coverage;Format=cobertura\"");

            // 4. Consolida todos os arquivos XML em um relatório unificado
            Console.WriteLine("\n--> Gerando relatório unificado...");
            string reportArgs = $"-reports:\"**/coverage.cobertura.xml\" -targetdir:\"{reportDir}\" -reporttypes:Html;Cobertura";
            ExecutarComando("reportgenerator", reportArgs);

            // 5. Abre o relatório no navegador padrão
            string indexPath = Path.Combine(reportDir, "index.html");
            if (File.Exists(indexPath))
            {
                Console.WriteLine($"\n[Sucesso] Relatório gerado em: {indexPath}");
                AbrirNoNavegador(indexPath);
            }
            else
            {
                Console.WriteLine("\n[Erro] Não foi possível encontrar o arquivo index.html gerado.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void LimparDiretorios(string reportDir)
    {
        if (Directory.Exists(reportDir))
        {
            Directory.Delete(reportDir, true);
        }

        // Remove pastas de TestResults antigas
        foreach (var testResultFolder in Directory.GetDirectories(Directory.GetCurrentDirectory(), "TestResults", SearchOption.AllDirectories))
        {
            try { Directory.Delete(testResultFolder, true); } catch { }
        }
    }

    static void ExecutarComando(string comando, string argumentos)
    {
        var psi = new ProcessStartInfo
        {
            FileName = comando,
            Arguments = argumentos,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var processo = new Process { StartInfo = psi };
        processo.Start();

        // Exibe a saída do terminal em tempo real
        while (!processo.StandardOutput.EndOfStream)
        {
            Console.WriteLine(processo.StandardOutput.ReadLine());
        }

        processo.WaitForExit();
    }

    static void AbrirNoNavegador(string caminhoArquivo)
    {
        var psi = new ProcessStartInfo
        {
            FileName = caminhoArquivo,
            UseShellExecute = true
        };
        Process.Start(psi);
    }
}