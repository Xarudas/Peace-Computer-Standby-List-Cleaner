﻿using System.Runtime.InteropServices;

namespace PСSLC.Core
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct TokPriv1Luid
	{
		public int Count;

		public long Luid;

		public int Attr;
	}
}
