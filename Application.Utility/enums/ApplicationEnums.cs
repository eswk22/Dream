using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utility
{
    public enum LanguageIdentifier
    {
        CSharp,
        CSharpScript,
        VBNetScript,
        VBNet,
        IL
    }

    public enum DiagnosticSeverity
    {
        Hidden = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }

    public enum Gateway
    {
        netcool,
        mspoi,
        dbgw,
        mysql,
        oracle,
        db2,
        mssql,
        postgresql,
        informix,
        sybase,
        remedy,
        remedyx,
        tn3270,
        tn5250,
        vt,
        htmlconnect,
        pdf,
        exchange,
        itncm,
        tsrm,
        xmpp,
        servicenow,
        salesforce,
        snmp,
        hpom,
        ews,
        wsliteconnect,
        emailconnect2,
        email,
        ftpconnect,
        networkconnect,
        hpsm,
        http,
        caspectrum,
        tibcobespoke

    }

    public enum ParamType
    {
        Input,
        Output
    }

    public enum SourceType
    {
        Default,
        Constant,
        Output,
        Flow,
        CNS,
        WSData,
        Param,
        Property
    }

    public enum MergeType
    {
        Any,
        All,
        Targets
    }

    public enum ConnectorType
    {
        Good,
        Bad,
        None
    }

    public enum PointType
    {
        General,
        SourcePoint,
        TargetPoint,
        None
    }
}
