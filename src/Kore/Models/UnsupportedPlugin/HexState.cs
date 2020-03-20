﻿using System.IO;
using Kontract.Interfaces.FileSystem;
using Kontract.Interfaces.Plugins.State;
using Kontract.Interfaces.Progress;
using Kontract.Interfaces.Providers;
using Kontract.Models.IO;

namespace Kore.Models.UnsupportedPlugin
{
    public class HexState : IHexState, ILoadFiles
    {
        public Stream FileStream { get; private set; }

        public async void Load(IFileSystem fileSystem, UPath filePath, ITemporaryStreamProvider temporaryStreamProvider,
            IProgressContext progress)
        {
            FileStream = await fileSystem.OpenFileAsync(filePath);
        }
    }
}
