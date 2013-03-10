using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

namespace DLLInjectorCS
{
    public delegate void InjectedCallback();

    class Injector
    {
        public string fileName;
        private IntPtr procHandle;
        private IntPtr loadLibAddr;
        private string procName;
        private static Thread curThread;
        private InjectedCallback injectedCb;

        public bool suspendOnInject;

        public void setTargetById(int id)
        {
            procHandle = Natives.OpenProcess(Natives.ProcessAccessFlags.All, false, id);
        }

        public bool injectDll()
        {
            byte[] flnBuf = System.Text.Encoding.ASCII.GetBytes(fileName);
            uint flnSize = (uint)flnBuf.Length;
            UIntPtr outBuf;
            IntPtr newMem;
            IntPtr remoteThread;
            bool result = false;
                
            newMem = Natives.VirtualAllocEx(procHandle, (IntPtr)0, flnSize, Natives.AllocationType.Commit, Natives.MemoryProtection.ReadWrite);
            if (Natives.WriteProcessMemory(procHandle, newMem, flnBuf, flnSize, out outBuf) == false)
                return false;
            remoteThread = Natives.CreateRemoteThread(procHandle, (IntPtr)0, 0, loadLibAddr, newMem, 0, (IntPtr)0);
            if (remoteThread != (IntPtr)0)
                result = (bool)(Natives.WaitForSingleObject(remoteThread, 10000) != Natives.WAIT_TIMEOUT);
            else
                MessageBox.Show(Marshal.GetLastWin32Error().ToString());


            Natives.VirtualFreeEx(procHandle, newMem, flnSize, Natives.FreeType.Decommit);

            return result;
        }

        private void injectOnProcStart()
        {
            try
            {
                //So if procName changes during the wait it won't change in this thread
                string t = string.Copy(procName);
                Process[] procs = Process.GetProcessesByName(t);
                while (procs.Length == 0)
                {
                    Thread.Sleep(1000);
                    procs = Process.GetProcessesByName(t);
                }
                setTargetById(procs[0].Id);
                if (suspendOnInject)
                {
                    suspendProcess(procs[0]);
                }

                injectDll();
                if (injectedCb != null)
                    injectedCb();


                if (suspendOnInject)
                {
                    resumeProcess(procs[0]);
                }
            }
            catch (ThreadAbortException e)
            {
                return;
            }
        }

        public void startInjectingThread(string s, InjectedCallback cb)
        {
            if (curThread != null)
                curThread.Abort();
            procName = s;
            injectedCb = cb;
            curThread = new Thread(new ThreadStart(injectOnProcStart));
            curThread.Start();
        }

        public static Thread getThread()
        {
            return curThread;
        }

        public void suspendProcess(Process target)
        {
            foreach (ProcessThread pt in target.Threads)
            {
                IntPtr hThread = Natives.OpenThread(Natives.ThreadAccess.SUSPEND_RESUME, false, (uint)pt.Id);
                Natives.SuspendThread(hThread);
                Natives.CloseHandle(hThread);
            }
        }

        public void resumeProcess(Process target)
        {
            foreach (ProcessThread pt in target.Threads)
            {
                IntPtr hThread = Natives.OpenThread(Natives.ThreadAccess.SUSPEND_RESUME, false, (uint)pt.Id);
                Natives.ResumeThread(hThread);
                Natives.CloseHandle(hThread);
            }
        }

        public Injector()
        {
            IntPtr kernel32 = Natives.GetModuleHandle("Kernel32");
            loadLibAddr = Natives.GetProcAddress(kernel32, "LoadLibraryA");
            suspendOnInject = false;
        }

        public Injector(string fln) : this()
        {
            this.fileName = fln;
        }

        public Injector(string fln, int id)
            : this(fln)
        {
            setTargetById(id);
        }
    }
}
