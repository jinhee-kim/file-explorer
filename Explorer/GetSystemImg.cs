using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Explorer
{
    class GetSystemImg
    {
        [DllImport("Shell32.dll")]
        private static extern int SHGetFileInfo(
                string pszPath,         // 경로 버퍼
                uint dwFileAttributes,  // 파일 속성의 조합
                out SHFILEINFO psfi,    // 구조체의 주소(반환값)
                uint cbFileInfo,        // 위 구조체의 크기
                SHGFI uFlags            // 이 플래그에 의해서 함수의 행동과 정보가 결정됨.
                );

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;  // 파일 아이콘의 핸들
            public int iIcon;     // 
            public uint dwAttributes;   //
            [MarshalAs(UnmanagedType.LPStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.LPStr, SizeConst = 80)]
            public string szTypeName;
        };
        
        private enum SHGFI
        {
            LargeIcon = 0x00000000,
            SmallIcon = 0x00000001,
            SelectIcon = 0x00000002,
            UseFileAttributes = 0x00000010,
            Icon = 0x00000100,
            DisplayName = 0x00000200,
            Typename = 0x00000400,
            SysIconIndex = 0x00004000,
            LinkOverlay = 0x00008000
        }

        /// <summary>
        /// 파일/폴더 이미지 얻기
        /// </summary>
        /// <param name="pszPath">파일/폴더 경로</param>
        /// <param name="bBigSmall">큰이미지/작은이미지</param>
        /// <param name="bSelect">선택 이미지</param>
        /// <returns></returns>
        public Icon GetIcon(String pszPath, bool bBigSmall, bool bSelect)
        {
            SHGFI uFlags;
            SHFILEINFO psfi = new SHFILEINFO();
            int cbFileInfo = Marshal.SizeOf(psfi);

            if (bBigSmall)
                uFlags = SHGFI.Icon | SHGFI.LargeIcon;
            else
                uFlags = SHGFI.Icon | SHGFI.SmallIcon;

            if (bSelect)
                uFlags = uFlags | SHGFI.SelectIcon;

            // 폴더/파일 아이콘 얻기
            SHGetFileInfo(pszPath, 256, out psfi, (uint)cbFileInfo, uFlags);

            // 아이콘 형식으로 반환
            return Icon.FromHandle(psfi.hIcon);
        }
    }
}
