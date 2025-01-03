﻿using Crestron.SimplSharpPro.DeviceSupport;
using System;

namespace CrestronHelperLibrary.SmartGraphics.Joins
{
	public class DigitalJoin : IJoin<bool>
	{
		protected readonly BasicTriList triList;
		public readonly uint id;

		internal DigitalJoin(BasicTriList triList, uint id)
		{
			this.triList = triList;
			this.id = id;
		}

		public bool Value
		{
			get => triList.BooleanOutput[id].BoolValue;
			set => triList.BooleanInput[id].BoolValue = value;
		}

		public event Action<bool>? OnChange;

		internal void Change(bool value) => OnChange?.Invoke(value);
	}
}
