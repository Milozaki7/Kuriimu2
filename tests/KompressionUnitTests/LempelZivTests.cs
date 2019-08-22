﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Kompression.LempelZiv;
using Kompression.Specialized.SlimeMoriMori;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KompressionUnitTests
{
    [TestClass]
    public class LempelZivTests
    {
        //[TestMethod]
        //public void FindOccurences_Naive_IsCorrect()
        //{
        //    var input = new byte[]
        //        {0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00};

        //    var lzFinder = new LzOccurrenceFinder(LzMode.Naive, 8, 4, 16);
        //    var results = lzFinder.Process(new MemoryStream(input)).OrderBy(x => x.Position).ToList();

        //    Assert.AreEqual(1, results.Count);
        //    Assert.AreEqual(7, results[0].Position);
        //    Assert.AreEqual(8, results[0].Length);
        //    Assert.AreEqual(7, results[0].Displacement);
        //}

        //[TestMethod]
        //public void FindOccurences_SuffixTree_IsCorrect()
        //{
        //    var input = new byte[]
        //        {0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00};

        //    var lzFinder = new LzOccurrenceFinder(LzMode.SuffixTree, input.Length, 1, 8);
        //    var results = lzFinder.Process(new MemoryStream(input)).OrderBy(x => x.Position).ToList();

        //    Assert.AreEqual(5, results.Count);
        //    Assert.AreEqual(12, (int)results[4].Position);
        //    Assert.AreEqual(8, (int)results[4].Displacement);
        //    Assert.AreEqual(3, results[4].Length);
        //}

        //[TestMethod]
        //public void SuffixArray_IsCorrect()
        //{
        //    var input = new byte[]
        //        {0x6d, 0x69, 0x73, 0x73, 0x69, 0x73, 0x73, 0x69, 0x70, 0x70, 0x69};

        //    var array = SuffixArray.Create(new MemoryStream(input));

        //    Assert.AreEqual(10, array.Suffixes[0]);
        //    Assert.AreEqual(7, array.Suffixes[1]);
        //    Assert.AreEqual(4, array.Suffixes[2]);
        //    Assert.AreEqual(1, array.Suffixes[3]);
        //    Assert.AreEqual(0, array.Suffixes[4]);
        //    Assert.AreEqual(9, array.Suffixes[5]);
        //    Assert.AreEqual(8, array.Suffixes[6]);
        //    Assert.AreEqual(6, array.Suffixes[7]);
        //    Assert.AreEqual(3, array.Suffixes[8]);
        //    Assert.AreEqual(5, array.Suffixes[9]);
        //    Assert.AreEqual(2, array.Suffixes[10]);
        //}

        //[TestMethod]
        //public void FindOccurrences_SuffixArray_IsCorrect()
        //{
        //    var input = new byte[]
        //        {0x6d, 0x69, 0x73, 0x73, 0x69, 0x73, 0x73, 0x69, 0x70, 0x70, 0x69};

        //    var finder = new LzOccurrenceFinder(LzMode.SuffixArray, input.Length, 1, input.Length);
        //    var results = finder.Process(new MemoryStream(input));

        //    ;
        //}


        //[TestMethod]
        //public void LZ10_CompressDecompress()
        //{
        //    var input = new byte[]
        //    {
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x70, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x04, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x05, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x09, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x89, 0x00
        //    };

        //    var compStream = new MemoryStream();
        //    var decompStream = new MemoryStream();

        //    LZ10.Compress(new MemoryStream(input), compStream);
        //    compStream.Position = 0;
        //    LZ10.Decompress(compStream, decompStream);
        //    compStream.Position = decompStream.Position = 0;

        //    Assert.AreEqual(0x10, compStream.ToArray()[0]);
        //    Assert.IsTrue(input.SequenceEqual(decompStream.ToArray()));
        //}

        //[TestMethod]
        //public void LZ11_CompressDecompress()
        //{
        //    var input = new byte[]
        //    {
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x70, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x04, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x05, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x09, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x89, 0x00
        //    };

        //    var compStream = new MemoryStream();
        //    var decompStream = new MemoryStream();

        //    LZ11.Compress(new MemoryStream(input), compStream);
        //    compStream.Position = 0;
        //    LZ11.Decompress(compStream, decompStream);
        //    compStream.Position = decompStream.Position = 0;

        //    Assert.AreEqual(0x11, compStream.ToArray()[0]);
        //    Assert.IsTrue(input.SequenceEqual(decompStream.ToArray()));
        //}

        //[TestMethod]
        //public void LZ40_CompressDecompress()
        //{
        //    var input = new byte[]
        //    {
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x70, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x04, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x05, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x09, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x89, 0x00
        //    };

        //    var compStream = new MemoryStream();
        //    var decompStream = new MemoryStream();

        //    LZ40.Compress(new MemoryStream(input), compStream);
        //    compStream.Position = 0;
        //    LZ40.Decompress(compStream, decompStream);
        //    compStream.Position = decompStream.Position = 0;

        //    Assert.AreEqual(0x40, compStream.ToArray()[0]);
        //    Assert.IsTrue(input.SequenceEqual(decompStream.ToArray()));
        //}

        //[TestMethod]
        //public void LZ60_CompressDecompress()
        //{
        //    var input = new byte[]
        //    {
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x70, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x04, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x05, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x09, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x89, 0x00
        //    };

        //    var compStream = new MemoryStream();
        //    var decompStream = new MemoryStream();

        //    LZ60.Compress(new MemoryStream(input), compStream);
        //    compStream.Position = 0;
        //    LZ60.Decompress(compStream, decompStream);
        //    compStream.Position = decompStream.Position = 0;

        //    Assert.AreEqual(0x60, compStream.ToArray()[0]);
        //    Assert.IsTrue(input.SequenceEqual(decompStream.ToArray()));
        //}

        //[TestMethod]
        //public void LZ77_CompressDecompress()
        //{
        //    var input = new byte[]
        //    {
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x70, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x04, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x05, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x09, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x89, 0x00
        //    };

        //    var compStream = new MemoryStream();
        //    var decompStream = new MemoryStream();

        //    LZ77.Compress(new MemoryStream(input), compStream);
        //    compStream.Position = 0;
        //    LZ77.Decompress(compStream, decompStream);
        //    compStream.Position = decompStream.Position = 0;

        //    Assert.IsTrue(input.SequenceEqual(decompStream.ToArray()));
        //}

        //[TestMethod]
        //public void LZSS_CompressDecompress()
        //{
        //    var input = new byte[]
        //    {
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x70, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x04, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x05, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x09, 0x00,
        //        0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x89, 0x00
        //    };

        //    var compStream = new MemoryStream();
        //    var decompStream = new MemoryStream();

        //    LZSS.Compress(new MemoryStream(input), compStream);
        //    compStream.Position = 0;
        //    LZSS.Decompress(compStream, decompStream);
        //    compStream.Position = decompStream.Position = 0;

        //    var decompressedSize = compStream.ToArray()[0xC] | (compStream.ToArray()[0xD] << 8) | (compStream.ToArray()[0xE] << 16) | (compStream.ToArray()[0xF] << 24);

        //    Assert.AreEqual(0x53, compStream.ToArray()[0]);
        //    Assert.AreEqual(0x53, compStream.ToArray()[1]);
        //    Assert.AreEqual(0x5A, compStream.ToArray()[2]);
        //    Assert.AreEqual(0x4C, compStream.ToArray()[3]);
        //    Assert.AreEqual((int)decompStream.Length, decompressedSize);
        //    Assert.IsTrue(input.SequenceEqual(decompStream.ToArray()));
        //}

        [TestMethod]
        public void LZSSVLC_CompressDecompress()
        {
            var input = new byte[]
            {
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x70, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x04, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x05, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x09, 0x00,
                0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x89, 0x00
            };

            var compStream = new MemoryStream();
            var decompStream = new MemoryStream();

            new LzssVlc().Compress(new MemoryStream(input), compStream);
            compStream.Position = 0;
            new LzssVlc().Decompress(compStream, decompStream);
            compStream.Position = decompStream.Position = 0;

            Assert.IsTrue(input.SequenceEqual(decompStream.ToArray()));
        }

        [TestMethod]
        public void Stub_Slime_Compress()
        {
            var file = @"D:\Users\Kirito\Desktop\compressedBlob1.bin.decomp";
            var str = File.OpenRead(file);
            var save = File.Create(file + ".comp");

            var watch = new Stopwatch();
            watch.Start();
            new SlimeMoriMoriCompression(2, 5).Compress(str, save);
            watch.Stop();

            save.Close();
        }

        [TestMethod]
        public void Stub_Slime_Decompress()
        {
            var file = @"D:\Users\Kirito\Desktop\compressedBlob1.bin.decomp.comp";
            var str = File.OpenRead(file);
            var save = File.Create(file + ".decomp");

            var watch = new Stopwatch();
            watch.Start();
            new SlimeMoriMoriCompression(2, 5).Decompress(str, save);
            watch.Stop();

            save.Close();
        }

        [TestMethod]
        public void Stub_LZECD_Decompress()
        {
            var file = @"D:\Users\Kirito\Desktop\test_ecd.bin";
            //var file = @"D:\Users\Kirito\Desktop\vt1.file1.bin.new2";
            var str = File.OpenRead(file);
            var save = File.Create(file + ".decomp");

            new LzEcd().Decompress(str, save);

            save.Close();
        }
    }
}
