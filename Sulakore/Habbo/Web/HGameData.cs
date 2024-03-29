﻿using System.Diagnostics;
using System.Text.RegularExpressions;

using Sulakore.Communication;

namespace Sulakore.Habbo.Web;

[DebuggerDisplay("Host: {InfoHost}, Port(s): {InfoPort}")]
public class HGameData
{
    private readonly Dictionary<string, string> _variables;

    private const string FLASH_VAR_PATTERN = "(\"|')+?(?<var>.*?)(\"|')+?(:| :| : |: |,|, )+?(\"|')+?(?<value>.*?)(\"|')+(\\)|,|\\s|$)+";

    private string _source;
    public string Source
    {
        get => _source;
        set
        {
            _source = value;
            ExtractVariables();
        }
    }

    public HHotel Hotel { get; private set; }

    public string this[string variable]
    {
        get
        {
            _variables.TryGetValue(variable, out string value);
            return value;
        }
    }
    public string InfoHost => this["connection.info.host"];
    public string InfoPort => this["connection.info.port"];

    public HGameData()
    {
        _variables = new Dictionary<string, string>();
    }
    public HGameData(string source)
        : this()
    {
        Source = source;
    }

    public bool ContainsVariable(string variable)
    {
        return _variables.ContainsKey(variable);
    }

    private void ExtractVariables()
    {
        _variables.Clear();

        MatchCollection matches = Regex.Matches(Source, FLASH_VAR_PATTERN,
            RegexOptions.Multiline
#if !DEBUG
            | RegexOptions.Compiled
#endif
            );

        foreach (Match match in matches)
        {
            string variable = match.Groups["var"].Value;
            string value = match.Groups["value"].Value;

            _variables[variable] = value.Replace("\\/", "/");
        }
        Hotel = HotelEndPoint.GetHotel(InfoHost);
    }
}