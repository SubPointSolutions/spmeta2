using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Content type IDs generated on top of Microsoft.SharePoint.SPBuiltInContentTypeId class.
    /// </summary>
    public static class BuiltInContentTypeId
    {
        public static readonly string AdminTask = "0x010802";
        public static readonly string Announcement = "0x0104";
        public static readonly string BasicPage = "0x010109";
        public static readonly string BlogComment = "0x0111";
        public static readonly string BlogPost = "0x0110";
        public static readonly string CallTracking = "0x0100807FBAC5EB8A4653B8D24775195B5463";
        public static readonly string Contact = "0x0106";
        public static readonly string Discussion = "0x012002";
        public static readonly string DisplayTemplateJS = "0x0101002039C03B61C64EC4A04F5361F3851068";
        public static readonly string Document = "0x0101";

        [Obsolete("Correct 'Document Set' content type ID is '0x0120D520. Please use DocumentSet_Correct property instead. More details - https://github.com/SubPointSolutions/spmeta2/issues/157'")]
        public static readonly string DocumentSet = "0x0120D5";
        public static readonly string DocumentSet_Correct = "0x0120D520";

        public static readonly string DocumentWorkflowItem = "0x010107";
        public static readonly string DomainGroup = "0x010C";
        public static readonly string DublinCoreName = "0x01010B";
        public static readonly string Event = "0x0102";
        public static readonly string FarEastContact = "0x0116";
        public static readonly string Folder = "0x0120";
        public static readonly string GbwCirculationCTName = "0x01000F389E14C9CE4CE486270B9D4713A5D6";
        public static readonly string GbwOfficialNoticeCTName = "0x01007CE30DD1206047728BAFD1C39A850120";
        public static readonly string HealthReport = "0x0100F95DB3A97E8046B58C6A54FB31F2BD46";
        public static readonly string HealthRuleDefinition = "0x01003A8AA7A4F53046158C5ABD98036A01D5";
        public static readonly string Holiday = "0x01009BE2AB5291BF4C1A986910BD278E4F18";
        public static readonly string IMEDictionaryItem = "0x010018F21907ED4E401CB4F14422ABC65304";
        public static readonly string Issue = "0x0103";
        public static readonly string Item = "0x01";
        public static readonly string Link = "0x0105";
        public static readonly string LinkToDocument = "0x01010A";
        public static readonly string MasterPage = "0x010105";
        public static readonly string MasterPagePreview = "0x010106";
        public static readonly string Message = "0x0107";
        public static readonly string ODCDocument = "0x010100629D00608F814DD6AC8A86903AEE72AA";
        public static readonly string Person = "0x010A";
        public static readonly string Picture = "0x010102";
        public static readonly string Resource = "0x01004C9F4486FBF54864A7B0A33D02AD19B1";
        public static readonly string ResourceGroup = "0x0100CA13F2F8D61541B180952DFB25E3E8E4";
        public static readonly string ResourceReservation = "0x0102004F51EFDEA49C49668EF9C6744C8CF87D";
        public static readonly string RootOfList = "0x012001";
        public static readonly string Schedule = "0x0102007DBDC1392EAF4EBBBF99E41D8922B264";
        public static readonly string ScheduleAndResourceReservation = "0x01020072BB2A38F0DB49C3A96CF4FA85529956";
        public static readonly string SharePointGroup = "0x010B";
        public static readonly string SummaryTask = "0x012004";
        public static readonly string System = "0x";
        public static readonly string Task = "0x0108";
        public static readonly string Timecard = "0x0100C30DDA8EDB2E434EA22D793D9EE42058";
        public static readonly string UDCDocument = "0x010100B4CBD48E029A4AD8B62CB0E41868F2B0";
        public static readonly string UntypedDocument = "0x010104";
        public static readonly string WebPartPage = "0x01010901";
        public static readonly string WhatsNew = "0x0100A2CA87FF01B442AD93F37CD7DD0943EB";
        public static readonly string Whereabouts = "0x0100FBEEE6F0C500489B99CDA6BB16C398F7";
        public static readonly string WikiDocument = "0x010108";
        public static readonly string WorkflowHistory = "0x0109";
        public static readonly string WorkflowTask = "0x010801";
        public static readonly string XMLDocument = "0x010101";
        public static readonly string XSLStyle = "0x010100734778F2B7DF462491FC91844AE431CF";


    }
}
