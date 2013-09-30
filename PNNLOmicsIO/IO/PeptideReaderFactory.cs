﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmicsIO.IO
{
    public class PeptideReaderFactory
    {
        public static ISequenceFileReader CreateReader(string path)
        {
            if (path == null)
                return null;

            SequenceFileType type =  GetFileType(path);

            if (type == SequenceFileType.None)
                return null; 
             
            return CreateReader(type);
        }
        public static ISequenceFileReader CreateReader(SequenceFileType type)
        {
            ISequenceFileReader reader = null;

            switch (type)
            {
                case SequenceFileType.SEQUESTFirstHit:
                    break;
                case SequenceFileType.MSGF:
                    reader = new MsgfReader();
                    break;
                case SequenceFileType.SkylineTransitionFile:
                    reader = new SkylineTransitionFileReader();
                    break;
                default:
                    break;
            }

            return reader;
        }


        private static SequenceFileType GetFileType(string peptidePath)
        {
            SequenceFileType type = SequenceFileType.None;
            string lowerPath      = peptidePath.ToLower();
           
            if (lowerPath.EndsWith("msgfdb_fht.txt"))
            {
                    type = SequenceFileType.MSGF;
            }
            else if (lowerPath.EndsWith("fht_msgf.txt"))
            {
                    type  = SequenceFileType.MSGF;
            }
            else if (lowerPath.EndsWith("syn.txt"))
            {
                    type = SequenceFileType.SEQUESTSynopsis;
            }
            else if (lowerPath.EndsWith("fht.txt"))
            {
                    type = SequenceFileType.SEQUESTFirstHit;                    
            }
            return type;
        }
    }

    /// <summary>
    /// Types of peptide sequence fiels to read.
    /// </summary>
    public enum SequenceFileType
    {
        SEQUESTFirstHit,
        SEQUESTSynopsis,
        MSGF,
        SkylineTransitionFile,
        XTandem,
        None
    }
}
