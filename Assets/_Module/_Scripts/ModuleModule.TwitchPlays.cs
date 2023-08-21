using System.Collections;
using System.Text.RegularExpressions;

partial class ModuleModule
{
    // * TP Documentation: https://github.com/samfundev/KtaneTwitchPlays/wiki/External-Mod-Module-Support
#pragma warning disable IDE0051, IDE1006
    private readonly string TwitchHelpMessage = @"Use '!{0} breh' to do things | '!{0} breh2' to do other things; extra information here.";
    // private bool TwitchPlaysActive;
    // private bool TwitchShouldCancelCommand;
#pragma warning restore IDE1006

    private IEnumerator ProcessTwitchCommand(string command) {
        yield return "sendtochaterror TP has not yet been implemented.";

        //  command = command.Trim().ToUpperInvariant();

        // Match match;
        // if (IsMatch(command, @"PATTERN", out match)) {
        //     yield return null;
        //     //! Do stuff.
        //     yield break;
        // }

        // yield return "sendtochaterror Invalid command!";
    }

    private IEnumerator TwitchHandleForcedSolve() {
        Log("TP autosolver has not yet been implemented. Calling KMBombModule.HandlePass.");
        _module.HandlePass();
        yield return null;
    }
#pragma warning restore IDE0051

    private bool IsMatch(string command, string pattern, out Match match) {
        match = Regex.Match(command, pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        return match.Success;
    }
}