using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;


namespace Convert2PDF_WPF
{
    class DirectoryTree : INotifyPropertyChanged
    {
        public DirectoryTree()
        {
            this.isChecked = false;
            this.isDir = true;
        }
        public DirectoryTree(DirectoryTree parent, FileSystemInfo fi)
            : this()
        {
            this.Parent = parent;
            this.Info = fi;
        }
        public DirectoryTree(DirectoryTree parent, FileSystemInfo fi, Boolean bChecked)
            : this(parent, fi)
        {
            this.isChecked = bChecked;
        }
        // public DirectoryInfo Info { get; set; }
        public FileSystemInfo Info { get; set; }
        public DirectoryTree Parent { get; set; }
        public Boolean? IsChecked
        {
            get { return this.isChecked; }
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    NotifyPropertyChanged("IsChecked");
                    if (this.isChecked == true) // 如果节点被选中
                    {
                        if (this.dirs != null)
                            foreach (DirectoryTree dt in this.dirs)
                                dt.IsChecked = true;
                        if (this.Parent != null)
                        {
                            Boolean bExistUncheckedChildren = false;
                            foreach (DirectoryTree dt in this.Parent.Directories)
                                if (dt.IsChecked != true)
                                {
                                    bExistUncheckedChildren = true;
                                    break;
                                }
                            if (bExistUncheckedChildren)
                                this.Parent.IsChecked = null;
                            else
                                this.Parent.IsChecked = true;
                        }
                    }
                    else if (this.isChecked == false)   // 如果节点未选中
                    {
                        if (this.dirs != null)
                            foreach (DirectoryTree dt in this.dirs)
                                dt.IsChecked = false;
                        if (this.Parent != null)
                        {
                            Boolean bExistCheckedChildren = false;
                            foreach (DirectoryTree dt in this.Parent.Directories)
                                if (dt.IsChecked != false)
                                {
                                    bExistCheckedChildren = true;
                                    break;
                                }
                            if (bExistCheckedChildren)
                                this.Parent.IsChecked = null;
                            else
                                this.Parent.IsChecked = false;
                        }
                    }
                    else
                    {
                        if (this.Parent != null)
                            this.Parent.IsChecked = null;
                    }
                }
            }
        }
        public ObservableCollection<DirectoryTree> Directories
        {
            get
            {
                if (this.dirs == null)
                {
                    this.dirs = new ObservableCollection<DirectoryTree>();
                    try
                    {
                        // foreach (DirectoryInfo di in Info.GetDirectories())
                        //     this.dirs.Add(new DirectoryTree(this, di, this.isChecked == true));
                        if((Info.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            DirectoryInfo dinfo = new DirectoryInfo(Info.FullName);
                            foreach (FileSystemInfo fi in dinfo.GetFileSystemInfos())
                                this.dirs.Add(new DirectoryTree(this, fi, this.isChecked == true));
                        }
                        
                    }
                    catch
                    {

                    }
                }
                return dirs;
            }
        }


        public static ObservableCollection<DirectoryTree> InitRoot()
        {
            ObservableCollection<DirectoryTree> dts = new ObservableCollection<DirectoryTree>();
            DriveInfo[] drvs = DriveInfo.GetDrives();
            foreach (DriveInfo drv in drvs)
            {
                if (drv.DriveType == DriveType.Fixed)
                {
                    DirectoryTree dt = new DirectoryTree(null, drv.RootDirectory);
                    dts.Add(dt);
                }
            }
            return dts;
        }

        public static ObservableCollection<DirectoryTree> InitRootPath(string dir)
        {
            ObservableCollection<DirectoryTree> dts = new ObservableCollection<DirectoryTree>();

            DirectoryTree dt = new DirectoryTree(null, new DirectoryInfo(dir));
            dts.Add(dt);

            return dts;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        private Boolean? isChecked;
        private ObservableCollection<DirectoryTree> dirs;
        private Boolean isDir;
    }
}
