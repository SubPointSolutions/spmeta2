namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Reflects Microsoft.SharePoint.SPRegionalSettings.GlobalTimeZones eumeration
    /// https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.spregionalsettings.globaltimezones.aspx
    /// </summary>
    public static class BuiltInTimeZoneId
    {
        // pregenerated with the following PS script
        //$web = get-spweb "http://dev13:31415"
        //       $names = [Microsoft.SharePoint.SPRegionalSettings]::GlobalTimeZones | sort-object "Description"

        //foreach($name in $names) {

        //   $zoneArray = $name.Description.Split(@(") "), "RemoveEmptyEntries") 

        //   $zoneUTC =  $zoneArray[0]
        //   $zoneDescription =  $zoneArray[1].Replace(' ', '').Replace('-', '').Replace('.', '').Replace(',', '').Replace('(', '_').Replace(')', '').Replace("and", "And").Replace("'", "").Replace("+","_Plus_")

        //   $zoneUTC = $zoneUTC.Replace("(","").Replace(":","_").Replace("+","_Plus_").Replace("-","_Minus_")

        //   #"[$($name.Description)] -> [$zoneUTC] [$zoneDescription]"

        //   "/// <summary>"
        //   "/// ID:[$($name.ID)] - $($name.Description)"
        //   "/// </summary>"
        //   "public static ushort $zoneUTC" + "_" + "$zoneDescription = $($name.Id);"
        //   ""
        //}

        /// <summary>
        /// ID:[86] - (UTC) Casablanca
        /// </summary>
        public static ushort UTC_Casablanca = 86;

        /// <summary>
        /// ID:[93] - (UTC) Coordinated Universal Time
        /// </summary>
        public static ushort UTC_CoordinatedUniversalTime = 93;

        /// <summary>
        /// ID:[2] - (UTC) Dublin, Edinburgh, Lisbon, London
        /// </summary>
        public static ushort UTC_DublinEdinburghLisbonLondon = 2;

        /// <summary>
        /// ID:[31] - (UTC) Monrovia, Reykjavik
        /// </summary>
        public static ushort UTC_MonroviaReykjavik = 31;

        /// <summary>
        /// ID:[4] - (UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna
        /// </summary>
        public static ushort UTC_Plus_01_00_AmsterdamBerlinBernRomeStockholmVienna = 4;

        /// <summary>
        /// ID:[6] - (UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague
        /// </summary>
        public static ushort UTC_Plus_01_00_BelgradeBratislavaBudapestLjubljanaPrague = 6;

        /// <summary>
        /// ID:[3] - (UTC+01:00) Brussels, Copenhagen, Madrid, Paris
        /// </summary>
        public static ushort UTC_Plus_01_00_BrusselsCopenhagenMadridParis = 3;

        /// <summary>
        /// ID:[57] - (UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb
        /// </summary>
        public static ushort UTC_Plus_01_00_SarajevoSkopjeWarsawZagreb = 57;

        /// <summary>
        /// ID:[69] - (UTC+01:00) West Central Africa
        /// </summary>
        public static ushort UTC_Plus_01_00_WestCentralAfrica = 69;

        /// <summary>
        /// ID:[83] - (UTC+01:00) Windhoek
        /// </summary>
        public static ushort UTC_Plus_01_00_Windhoek = 83;

        /// <summary>
        /// ID:[79] - (UTC+02:00) Amman
        /// </summary>
        public static ushort UTC_Plus_02_00_Amman = 79;

        /// <summary>
        /// ID:[5] - (UTC+02:00) Athens, Bucharest, Istanbul
        /// </summary>
        public static ushort UTC_Plus_02_00_AthensBucharestIstanbul = 5;

        /// <summary>
        /// ID:[80] - (UTC+02:00) Beirut
        /// </summary>
        public static ushort UTC_Plus_02_00_Beirut = 80;

        /// <summary>
        /// ID:[49] - (UTC+02:00) Cairo
        /// </summary>
        public static ushort UTC_Plus_02_00_Cairo = 49;

        /// <summary>
        /// ID:[98] - (UTC+02:00) Damascus
        /// </summary>
        public static ushort UTC_Plus_02_00_Damascus = 98;

        /// <summary>
        /// ID:[104] - (UTC+02:00) E. Europe
        /// </summary>
        public static ushort UTC_Plus_02_00_EEurope = 104;

        /// <summary>
        /// ID:[50] - (UTC+02:00) Harare, Pretoria
        /// </summary>
        public static ushort UTC_Plus_02_00_HararePretoria = 50;

        /// <summary>
        /// ID:[59] - (UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius
        /// </summary>
        public static ushort UTC_Plus_02_00_HelsinkiKyivRigaSofiaTallinnVilnius = 59;

        /// <summary>
        /// ID:[101] - (UTC+02:00) Istanbul
        /// </summary>
        public static ushort UTC_Plus_02_00_Istanbul = 101;

        /// <summary>
        /// ID:[27] - (UTC+02:00) Jerusalem
        /// </summary>
        public static ushort UTC_Plus_02_00_Jerusalem = 27;

        /// <summary>
        /// ID:[7] - (UTC+02:00) Minsk (old)
        /// </summary>
        public static ushort UTC_Plus_02_00_Minsk_old = 7;

        /// <summary>
        /// ID:[26] - (UTC+03:00) Baghdad
        /// </summary>
        public static ushort UTC_Plus_03_00_Baghdad = 26;

        /// <summary>
        /// ID:[100] - (UTC+03:00) Kaliningrad, Minsk
        /// </summary>
        public static ushort UTC_Plus_03_00_KaliningradMinsk = 100;

        /// <summary>
        /// ID:[74] - (UTC+03:00) Kuwait, Riyadh
        /// </summary>
        public static ushort UTC_Plus_03_00_KuwaitRiyadh = 74;

        /// <summary>
        /// ID:[56] - (UTC+03:00) Nairobi
        /// </summary>
        public static ushort UTC_Plus_03_00_Nairobi = 56;

        /// <summary>
        /// ID:[25] - (UTC+03:30) Tehran
        /// </summary>
        public static ushort UTC_Plus_03_30_Tehran = 25;

        /// <summary>
        /// ID:[24] - (UTC+04:00) Abu Dhabi, Muscat
        /// </summary>
        public static ushort UTC_Plus_04_00_AbuDhabiMuscat = 24;

        /// <summary>
        /// ID:[54] - (UTC+04:00) Baku
        /// </summary>
        public static ushort UTC_Plus_04_00_Baku = 54;

        /// <summary>
        /// ID:[51] - (UTC+04:00) Moscow, St. Petersburg, Volgograd
        /// </summary>
        public static ushort UTC_Plus_04_00_MoscowStPetersburgVolgograd = 51;

        /// <summary>
        /// ID:[89] - (UTC+04:00) Port Louis
        /// </summary>
        public static ushort UTC_Plus_04_00_PortLouis = 89;

        /// <summary>
        /// ID:[82] - (UTC+04:00) Tbilisi
        /// </summary>
        public static ushort UTC_Plus_04_00_Tbilisi = 82;

        /// <summary>
        /// ID:[84] - (UTC+04:00) Yerevan
        /// </summary>
        public static ushort UTC_Plus_04_00_Yerevan = 84;

        /// <summary>
        /// ID:[48] - (UTC+04:30) Kabul
        /// </summary>
        public static ushort UTC_Plus_04_30_Kabul = 48;

        /// <summary>
        /// ID:[87] - (UTC+05:00) Islamabad, Karachi
        /// </summary>
        public static ushort UTC_Plus_05_00_IslamabadKarachi = 87;

        /// <summary>
        /// ID:[47] - (UTC+05:00) Tashkent
        /// </summary>
        public static ushort UTC_Plus_05_00_Tashkent = 47;

        /// <summary>
        /// ID:[23] - (UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi
        /// </summary>
        public static ushort UTC_Plus_05_30_ChennaiKolkataMumbaiNewDelhi = 23;

        /// <summary>
        /// ID:[66] - (UTC+05:30) Sri Jayawardenepura
        /// </summary>
        public static ushort UTC_Plus_05_30_SriJayawardenepura = 66;

        /// <summary>
        /// ID:[62] - (UTC+05:45) Kathmandu
        /// </summary>
        public static ushort UTC_Plus_05_45_KathmAndu = 62;

        /// <summary>
        /// ID:[71] - (UTC+06:00) Astana
        /// </summary>
        public static ushort UTC_Plus_06_00_Astana = 71;

        /// <summary>
        /// ID:[102] - (UTC+06:00) Dhaka
        /// </summary>
        public static ushort UTC_Plus_06_00_Dhaka = 102;

        /// <summary>
        /// ID:[58] - (UTC+06:00) Ekaterinburg
        /// </summary>
        public static ushort UTC_Plus_06_00_Ekaterinburg = 58;

        /// <summary>
        /// ID:[61] - (UTC+06:30) Yangon (Rangoon)
        /// </summary>
        public static ushort UTC_Plus_06_30_Yangon_Rangoon = 61;

        /// <summary>
        /// ID:[22] - (UTC+07:00) Bangkok, Hanoi, Jakarta
        /// </summary>
        public static ushort UTC_Plus_07_00_BangkokHanoiJakarta = 22;

        /// <summary>
        /// ID:[46] - (UTC+07:00) Novosibirsk
        /// </summary>
        public static ushort UTC_Plus_07_00_Novosibirsk = 46;

        /// <summary>
        /// ID:[45] - (UTC+08:00) Beijing, Chongqing, Hong Kong, Urumqi
        /// </summary>
        public static ushort UTC_Plus_08_00_BeijingChongqingHongKongUrumqi = 45;

        /// <summary>
        /// ID:[64] - (UTC+08:00) Krasnoyarsk
        /// </summary>
        public static ushort UTC_Plus_08_00_Krasnoyarsk = 64;

        /// <summary>
        /// ID:[21] - (UTC+08:00) Kuala Lumpur, Singapore
        /// </summary>
        public static ushort UTC_Plus_08_00_KualaLumpurSingapore = 21;

        /// <summary>
        /// ID:[73] - (UTC+08:00) Perth
        /// </summary>
        public static ushort UTC_Plus_08_00_Perth = 73;

        /// <summary>
        /// ID:[75] - (UTC+08:00) Taipei
        /// </summary>
        public static ushort UTC_Plus_08_00_Taipei = 75;

        /// <summary>
        /// ID:[94] - (UTC+08:00) Ulaanbaatar
        /// </summary>
        public static ushort UTC_Plus_08_00_Ulaanbaatar = 94;

        /// <summary>
        /// ID:[63] - (UTC+09:00) Irkutsk
        /// </summary>
        public static ushort UTC_Plus_09_00_Irkutsk = 63;

        /// <summary>
        /// ID:[20] - (UTC+09:00) Osaka, Sapporo, Tokyo
        /// </summary>
        public static ushort UTC_Plus_09_00_OsakaSapporoTokyo = 20;

        /// <summary>
        /// ID:[72] - (UTC+09:00) Seoul
        /// </summary>
        public static ushort UTC_Plus_09_00_Seoul = 72;

        /// <summary>
        /// ID:[19] - (UTC+09:30) Adelaide
        /// </summary>
        public static ushort UTC_Plus_09_30_Adelaide = 19;

        /// <summary>
        /// ID:[44] - (UTC+09:30) Darwin
        /// </summary>
        public static ushort UTC_Plus_09_30_Darwin = 44;

        /// <summary>
        /// ID:[18] - (UTC+10:00) Brisbane
        /// </summary>
        public static ushort UTC_Plus_10_00_Brisbane = 18;

        /// <summary>
        /// ID:[76] - (UTC+10:00) Canberra, Melbourne, Sydney
        /// </summary>
        public static ushort UTC_Plus_10_00_CanberraMelbourneSydney = 76;

        /// <summary>
        /// ID:[43] - (UTC+10:00) Guam, Port Moresby
        /// </summary>
        public static ushort UTC_Plus_10_00_GuamPortMoresby = 43;

        /// <summary>
        /// ID:[42] - (UTC+10:00) Hobart
        /// </summary>
        public static ushort UTC_Plus_10_00_Hobart = 42;

        /// <summary>
        /// ID:[70] - (UTC+10:00) Yakutsk
        /// </summary>
        public static ushort UTC_Plus_10_00_Yakutsk = 70;

        /// <summary>
        /// ID:[41] - (UTC+11:00) Solomon Is., New Caledonia
        /// </summary>
        public static ushort UTC_Plus_11_00_SolomonIsNewCaledonia = 41;

        /// <summary>
        /// ID:[68] - (UTC+11:00) Vladivostok
        /// </summary>
        public static ushort UTC_Plus_11_00_Vladivostok = 68;

        /// <summary>
        /// ID:[17] - (UTC+12:00) Auckland, Wellington
        /// </summary>
        public static ushort UTC_Plus_12_00_AucklAndWellington = 17;

        /// <summary>
        /// ID:[97] - (UTC+12:00) Coordinated Universal Time+12
        /// </summary>
        public static ushort UTC_Plus_12_00_CoordinatedUniversalTime_Plus_12 = 97;

        /// <summary>
        /// ID:[40] - (UTC+12:00) Fiji
        /// </summary>
        public static ushort UTC_Plus_12_00_Fiji = 40;

        /// <summary>
        /// ID:[99] - (UTC+12:00) Magadan
        /// </summary>
        public static ushort UTC_Plus_12_00_Magadan = 99;

        /// <summary>
        /// ID:[92] - (UTC+12:00) Petropavlovsk-Kamchatsky - Old
        /// </summary>
        public static ushort UTC_Plus_12_00_PetropavlovskKamchatskyOld = 92;

        /// <summary>
        /// ID:[67] - (UTC+13:00) Nuku'alofa
        /// </summary>
        public static ushort UTC_Plus_13_00_Nukualofa = 67;

        /// <summary>
        /// ID:[16] - (UTC+13:00) Samoa
        /// </summary>
        public static ushort UTC_Plus_13_00_Samoa = 16;

        /// <summary>
        /// ID:[29] - (UTC-01:00) Azores
        /// </summary>
        public static ushort UTC_Minus_01_00_Azores = 29;

        /// <summary>
        /// ID:[53] - (UTC-01:00) Cape Verde Is.
        /// </summary>
        public static ushort UTC_Minus_01_00_CapeVerdeIs = 53;

        /// <summary>
        /// ID:[96] - (UTC-02:00) Coordinated Universal Time-02
        /// </summary>
        public static ushort UTC_Minus_02_00_CoordinatedUniversalTime02 = 96;

        /// <summary>
        /// ID:[30] - (UTC-02:00) Mid-Atlantic
        /// </summary>
        public static ushort UTC_Minus_02_00_MidAtlantic = 30;

        /// <summary>
        /// ID:[8] - (UTC-03:00) Brasilia
        /// </summary>
        public static ushort UTC_Minus_03_00_Brasilia = 8;

        /// <summary>
        /// ID:[85] - (UTC-03:00) Buenos Aires
        /// </summary>
        public static ushort UTC_Minus_03_00_BuenosAires = 85;

        /// <summary>
        /// ID:[32] - (UTC-03:00) Cayenne, Fortaleza
        /// </summary>
        public static ushort UTC_Minus_03_00_CayenneFortaleza = 32;

        /// <summary>
        /// ID:[60] - (UTC-03:00) Greenland
        /// </summary>
        public static ushort UTC_Minus_03_00_GreenlAnd = 60;

        /// <summary>
        /// ID:[90] - (UTC-03:00) Montevideo
        /// </summary>
        public static ushort UTC_Minus_03_00_Montevideo = 90;

        /// <summary>
        /// ID:[103] - (UTC-03:00) Salvador
        /// </summary>
        public static ushort UTC_Minus_03_00_Salvador = 103;

        /// <summary>
        /// ID:[28] - (UTC-03:30) Newfoundland
        /// </summary>
        public static ushort UTC_Minus_03_30_NewfoundlAnd = 28;

        /// <summary>
        /// ID:[91] - (UTC-04:00) Asuncion
        /// </summary>
        public static ushort UTC_Minus_04_00_Asuncion = 91;

        /// <summary>
        /// ID:[9] - (UTC-04:00) Atlantic Time (Canada)
        /// </summary>
        public static ushort UTC_Minus_04_00_AtlanticTime_Canada = 9;

        /// <summary>
        /// ID:[81] - (UTC-04:00) Cuiaba
        /// </summary>
        public static ushort UTC_Minus_04_00_Cuiaba = 81;

        /// <summary>
        /// ID:[33] - (UTC-04:00) Georgetown, La Paz, Manaus, San Juan
        /// </summary>
        public static ushort UTC_Minus_04_00_GeorgetownLaPazManausSanJuan = 33;

        /// <summary>
        /// ID:[65] - (UTC-04:00) Santiago
        /// </summary>
        public static ushort UTC_Minus_04_00_Santiago = 65;

        /// <summary>
        /// ID:[88] - (UTC-04:30) Caracas
        /// </summary>
        public static ushort UTC_Minus_04_30_Caracas = 88;

        /// <summary>
        /// ID:[35] - (UTC-05:00) Bogota, Lima, Quito
        /// </summary>
        public static ushort UTC_Minus_05_00_BogotaLimaQuito = 35;

        /// <summary>
        /// ID:[10] - (UTC-05:00) Eastern Time (US and Canada)
        /// </summary>
        public static ushort UTC_Minus_05_00_EasternTime_USAndCanada = 10;

        /// <summary>
        /// ID:[34] - (UTC-05:00) Indiana (East)
        /// </summary>
        public static ushort UTC_Minus_05_00_Indiana_East = 34;

        /// <summary>
        /// ID:[55] - (UTC-06:00) Central America
        /// </summary>
        public static ushort UTC_Minus_06_00_CentralAmerica = 55;

        /// <summary>
        /// ID:[11] - (UTC-06:00) Central Time (US and Canada)
        /// </summary>
        public static ushort UTC_Minus_06_00_CentralTime_USAndCanada = 11;

        /// <summary>
        /// ID:[37] - (UTC-06:00) Guadalajara, Mexico City, Monterrey
        /// </summary>
        public static ushort UTC_Minus_06_00_GuadalajaraMexicoCityMonterrey = 37;

        /// <summary>
        /// ID:[36] - (UTC-06:00) Saskatchewan
        /// </summary>
        public static ushort UTC_Minus_06_00_Saskatchewan = 36;

        /// <summary>
        /// ID:[38] - (UTC-07:00) Arizona
        /// </summary>
        public static ushort UTC_Minus_07_00_Arizona = 38;

        /// <summary>
        /// ID:[77] - (UTC-07:00) Chihuahua, La Paz, Mazatlan
        /// </summary>
        public static ushort UTC_Minus_07_00_ChihuahuaLaPazMazatlan = 77;

        /// <summary>
        /// ID:[12] - (UTC-07:00) Mountain Time (US and Canada)
        /// </summary>
        public static ushort UTC_Minus_07_00_MountainTime_USAndCanada = 12;

        /// <summary>
        /// ID:[78] - (UTC-08:00) Baja California
        /// </summary>
        public static ushort UTC_Minus_08_00_BajaCalifornia = 78;

        /// <summary>
        /// ID:[13] - (UTC-08:00) Pacific Time (US and Canada)
        /// </summary>
        public static ushort UTC_Minus_08_00_PacificTime_USAndCanada = 13;

        /// <summary>
        /// ID:[14] - (UTC-09:00) Alaska
        /// </summary>
        public static ushort UTC_Minus_09_00_Alaska = 14;

        /// <summary>
        /// ID:[15] - (UTC-10:00) Hawaii
        /// </summary>
        public static ushort UTC_Minus_10_00_Hawaii = 15;

        /// <summary>
        /// ID:[95] - (UTC-11:00) Coordinated Universal Time-11
        /// </summary>
        public static ushort UTC_Minus_11_00_CoordinatedUniversalTime11 = 95;

        /// <summary>
        /// ID:[39] - (UTC-12:00) International Date Line West
        /// </summary>
        public static ushort UTC_Minus_12_00_InternationalDateLineWest = 39;


    }
}
