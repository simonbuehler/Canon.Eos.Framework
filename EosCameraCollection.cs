﻿using System;
using System.Collections.Generic;

using EDSDKLib;

namespace Canon.Eos.Framework
{
    public sealed class EosCameraCollection : EosDisposable, IEnumerable<EosCamera>
    {
        private readonly IntPtr _cameraList;
        private int _count = -1;

        internal EosCameraCollection()
        {
            EosAssert.NotOk(EDSDK.EdsGetCameraList(out _cameraList), "Failed to get cameras.");
        }

        protected internal override void DisposeUnmanaged()
        {
            if (_cameraList != IntPtr.Zero)
                EDSDK.EdsRelease(_cameraList);
            base.DisposeUnmanaged();
        }

        public int Count
        {
            get
            {
                if(_count < 0)                
                    EDSDK.EdsGetChildCount(_cameraList, out _count);
                return _count;
            }
        }

        public EosCamera this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                    throw new IndexOutOfRangeException();

                IntPtr camera;
                EosAssert.NotOk(EDSDK.EdsGetChildAtIndex(_cameraList, index, out camera), string.Format("Failed to get camera #{0}.", index+1));
                if (camera == IntPtr.Zero)
                    throw new EosException(EDSDK.EDS_ERR_DEVICE_NOT_FOUND, string.Format("Failed to get camera #{0}.", index+1));
                return new EosCamera(camera);
            }
        }

        #region IEnumerable<Camera> Members

        public IEnumerator<EosCamera> GetEnumerator()
        {
            for (var i = 0; i < this.Count; ++i)
                yield return this[i];
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}