using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurfergraphyApi.Utils
{
    public enum ErrorCode
    {
        /// <summary>
        /// 구매사진정보를 찾을 수 없습니다.
        /// </summary>
        PhotoSaveHistory_NoPhoto = 100,
        /// <summary>
        /// 구매에 필요한 Wave 가 부족합니다.
        /// </summary>
        PhotoSaveHistory_NoWave = 101
    }
}