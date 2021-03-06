﻿using System;
using System.IO;

namespace Conduit.Integration.Tests.Support
{
    class CleanRoom
    {
        private readonly string _tempDir;
        private string _pwd;
        private string _previousDir;
        private bool _noDeleteOnExit;

        public void NoDelete() 
        {
            _noDeleteOnExit = true;
        } 

        public CleanRoom(string tempDir)
        {
            _tempDir = Path.GetFullPath(Path.Combine(tempDir, Path.GetRandomFileName()));
        }

        public void Enter()
        {
            _previousDir = Directory.GetCurrentDirectory();
            _pwd = _tempDir;
            Directory.CreateDirectory(_pwd);
            Directory.SetCurrentDirectory(_pwd);
        }

        public void Exit()
        {
            Directory.SetCurrentDirectory(_previousDir);

            try
            {
                if (false == _noDeleteOnExit)
                {
                    Directory.Delete(_tempDir, true);
                }
            }
            catch (Exception)
            { }
        }
    }
}