// <copyright file="Downloader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.AutoUpdate.Downloader class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.AutoUpdate
{
    using System.ComponentModel;
    using System.IO;
    using System.Net;

    /// <summary>
    /// Provides mechanisms to download files from the web.
    /// </summary>
    public interface IDownloader
    {        
        /// <summary>
        /// Occurs when an asynchronous string download operation completes.
        /// </summary>
        event DownloadStringCompletedEventHandler AsyncStringDownloadCompleted;

        /// <summary>
        /// Occurs when an asynchronous data download operation completes.
        /// </summary>
        event DownloadDataCompletedEventHandler AsyncDataDownloadCompleted;

        /// <summary>
        /// Occurs when an asynchronous file download operation completes.
        /// </summary>
        event AsyncCompletedEventHandler AsyncDownloadCompleted;
       
        /// <summary>
        /// Occurs when an asynchronous download operation successfully transfers some
        /// or all of the data.
        /// </summary>
        event DownloadProgressChangedEventHandler AsyncDownloadProgressChanged;

        /// <summary>
        /// Downloads the data found at the given address.
        /// </summary>
        /// <param name="address">
        /// The uri of the data to download.
        /// </param>
        /// <returns>
        /// The downloaded data.
        /// </returns>
        Stream DownloadData( string address );

        /// <summary>
        /// Downloads the string found at the given address.
        /// </summary>
        /// <param name="address">
        /// The uri of the data to download.
        /// </param>
        /// <returns>
        /// The downloaded string.
        /// </returns>
        string DownloadString( string address );

        /// <summary>
        /// Downloads a file from the given address and saves it in a local file.
        /// This is a blocking call.
        /// </summary>
        /// <param name="address">
        /// The uri of the file to download.
        /// </param>
        /// <param name="localFileName">
        /// The name of the local file the downloaded file should be saved as.
        /// </param>
        void DownloadFile( string address, string localFileName );

        /// <summary>
        /// Downloads a file from the given address and saves it in a local file.
        /// This is an async call.
        /// </summary>
        /// <param name="address">
        /// The uri of the file to download.
        /// </param>
        /// <param name="localFileName">
        /// The name of the local file the downloaded file should be saved as.
        /// </param>
        void DownloadFileAsync( string address, string localFileName );

        /// <summary>
        /// Downloads a data from the given address and saves it in a local file.
        /// This is an async call.
        /// </summary>
        /// <param name="address">
        /// The uri of the data to download.
        /// </param>
        void DownloadDataAsync( string address );

        /// <summary>
        /// Downloads a string from the given address and saves it in a local file.
        /// This is an async call.
        /// </summary>
        /// <param name="address">
        /// The uri of the data to download.
        /// </param>
        void DownloadStringAsync( string address );
    }
}
