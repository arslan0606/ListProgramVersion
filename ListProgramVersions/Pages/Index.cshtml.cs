using Microsoft.AspNetCore.Mvc.RazorPages;
using CliWrap;
using CliWrap.Buffered;

namespace ListProgramVersions.Pages
{
    public class IndexModel : PageModel
    {
        public Dictionary<string, string> ProgramVersions { get; private set; } = new Dictionary<string, string>();

        public async Task OnGetAsync()
        {
            ProgramVersions = await GetInstalledProgramVersions();
        }

        private async Task<Dictionary<string, string>> GetInstalledProgramVersions()
        {
            var versions = new Dictionary<string, string>();

            // Commands to get program versions
            versions["Node.js"] = await GetProgramVersion("node", "--version");
            versions["Git"] = await GetProgramVersion("git", "--version");
            versions["Python"] = await GetProgramVersion("python", "--version");

            return versions;
        }

        private async Task<string> GetProgramVersion(string program, string arguments)
        {
            try
            {
                var result = await Cli.Wrap(program)
                                     .WithArguments(arguments)
                                     .ExecuteBufferedAsync();

                // white space handling from a comand-line program for consistent formatting 
                return result.StandardOutput.Trim();
            }
            catch
            {
                return "Either not Installed or Error";
            }
        }
    }
}
