// <copyright file="Downloader.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>Defines the Atom.AutoUpdate.Downloader class.</summary>
// <author>Paul Ennemoser (Tick)</author>

namespace Atom.AutoUpdate
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using System.Net.Cache;

    /// <summary>
    /// Implements mechanisms to download files from the web.
    /// </summary>
    public sealed class Downloader : IDownloader
    {       
        /// <summary>
        /// Occurs when an asynchronous data download operation completes.
        /// </summary>
        public event DownloadDataCompletedEventHandler AsyncDataDownloadCompleted
        {
            add
            {
                this.webClient.DownloadDataCompleted += value;
            }

            remove
            {
                this.webClient.DownloadDataCompleted -= value;
            }
        }

        /// <summary>
        /// Occurs when an asynchronous file download operation completes.
        /// </summary>
        public event AsyncCompletedEventHandler AsyncDownloadCompleted
        {
            add
            {
                this.webClient.DownloadFileCompleted += value;
            }

            remove
            {
                this.webClient.DownloadFileCompleted -= value;
            }
        }
        
        /// <summary>
        /// Occurs when an asynchronous string download operation completes.
        /// </summary>
        public event DownloadStringCompletedEventHandler AsyncStringDownloadCompleted
        {
            add
            {
                this.webClient.DownloadStringCompleted += value;
            }

            remove
            {
                this.webClient.DownloadStringCompleted -= value;
            }
        }

        /// <summary>
        /// Occurs when an asynchronous download operation successfully transfers some
        /// or all of the data.
        /// </summary>
        public event DownloadProgressChangedEventHandler AsyncDownloadProgressChanged
        {
            add
            {
                this.webClient.DownloadProgressChanged += value;
            }

            remove
            {
                this.webClient.DownloadProgressChanged -= value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Downloader class.
        /// </summary>
        public Downloader()
        {
            this.webClient = new WebClient() {
                CachePolicy = new RequestCachePolicy( RequestCacheLevel.BypassCache )            
            };
        }

        /// <summary>
        /// Downloads the data found at the given address.
        /// </summary>
        /// <param name="address">
        /// The uri of the data to download.
        /// </param>
        /// <returns>
        /// The downloaded data.
        /// </returns>
        public Stream DownloadData( string address )
        {
            byte[] bytes = this.webClient.DownloadData( address );
            return new MemoryStream( bytes ) ;
        }

        /// <summary>
        /// Downloads the string found at the given address.
        /// </summary>
        /// <param name="address">
        /// The uri of the data to download.
        /// </param>
        /// <returns>
        /// The downloaded string.
        /// </returns>
        public string DownloadString( string address )
        {
            return this.webClient.DownloadString( address );
        }

        /// <summary>
        /// Downloads the data found at the given address
        /// and saves it in a local file.
        /// </summary>
        /// <param name="address">
        /// The uri of the file to download.
        /// </param>
        /// <param name="localFileName">
        /// The name of the local file the downloaded file should be saved as.
        /// </param>
        public void DownloadFile( string address, string localFileName )
        {
            this.webClient.DownloadFile( address, localFileName );
        }

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
        public void DownloadFileAsync( string address, string localFileName )
        {
            this.webClient.DownloadFileAsync( new Uri( address ), localFileName );
        }

        /// <summary>
        /// Downloads a data from the given address and saves it in a local file.
        /// This is an async call.
        /// </summary>
        /// <param name="address">
        /// The uri of the data to download.
        /// </param>
        public void DownloadDataAsync( string address )
        {
            this.webClient.DownloadDataAsync( new Uri( address ) );
        }

        /// <summary>
        /// Downloads a string from the given address and saves it in a local file.
        /// This is an async call.
        /// </summary>
        /// <param name="address">
        /// The uri of the data to download.
        /// </param>
        public void DownloadStringAsync( string address )
        {
            this.webClient.DownloadStringAsync( new Uri( address ) );
        }

        /// <summary>
        /// Provides methods to download files.
        /// </summary>
        private readonly WebClient webClient;
    }
}