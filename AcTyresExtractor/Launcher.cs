namespace AcTyresExtractor
{
    using System;
    using Extractor;

    class Launcher
    {
        static void Main(string[] args)
        {
            FoldersExtractor foldersExtractor = new FoldersExtractor();
            foldersExtractor.ExtractDataFromAllSubFolders(@"i:\Downloads\AssettoCorsa\proTyres_Ini_Lut_Pack_90_all\apps\python\proTyres\cars_mod\");
            Console.ReadLine();
        }
    }
}
