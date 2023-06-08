﻿using BarcodeScanner.Mobile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace SampleApp.Maui.OCRImageCapture
{
    public class OCRImageCaptureViewModel : INotifyPropertyChanged
    {
        private bool _isScanning { get; set; }
        public bool IsScanning
        {
            get { return _isScanning; }
            set
            {
                _isScanning = value;
                OnPropertyChanged(nameof(IsScanning));
            }
        }

        private bool _allowScanning { get; set; }
        public bool AllowScanning
        {
            get { return _allowScanning; }
            set
            {
                _allowScanning = value;
                OnPropertyChanged(nameof(AllowScanning));
            }
        }

        private ICommand _onDetectCommand { get; set; }
        public ICommand OnDetectCommand
        {
            get { return _onDetectCommand; }
            set
            {
                _onDetectCommand = value;
                OnPropertyChanged(nameof(OnDetectCommand));
            }
        }

        public ICommand ScanCommand
        {
            get
            {
                return new Command(
                    execute: () => {
                        IsScanning = true;
                        AllowScanning = false;
                    });
            }
        }

        private int _scanInterval { get; set; }
        public int ScanInterval
        {
            get { return _scanInterval; }
            set
            {
                _scanInterval = value;
                OnPropertyChanged(nameof(ScanInterval));
            }
        }
        private string result { get; set; }
        public string Result
        {
            get { return result; }
            set
            {
                result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        private bool showCamera { get; set; }
        public bool ShowCamera
        {
            get
            {
                return showCamera;
            }

            set
            {
                showCamera = value;
                OnPropertyChanged();
            }
        }

        private bool showImage { get; set; }
        public bool ShowImage
        {
            get
            {
                return showImage;
            }
            set
            {
                showImage = value;
                OnPropertyChanged();
            }
        }

        private byte[] imageData { get; set; }
        public ImageSource CapturedImageSource
        {
            get
            {
                if (imageData == null)
                    return null;

                return ImageSource.FromStream(() =>
                {
                    return new MemoryStream(imageData);
                });
            }
        }

        public OCRImageCaptureViewModel()
        {
            this.ScanInterval = 1000;
            this.IsScanning = false;
            this.AllowScanning = true;
            this.OnDetectCommand = new Command<OnDetectedEventArg>(ExecuteOnDetectedCommand);
            this.Result = string.Empty;
            this.showCamera = true;
            this.showImage = false;
        }

        public void ExecuteOnDetectedCommand(OnDetectedEventArg arg)
        {
            this.Result = arg.OCRResult.AllText;

            imageData = arg.ImageData;
            OnPropertyChanged(nameof(CapturedImageSource));
            ShowCamera = false;
            ShowImage = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
        }
    }
}
