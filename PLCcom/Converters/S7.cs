using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCcom.Converters
{
    public static class S7
    {
        private static long bias = 621355968000000000L;

        private static int BCDtoByte(byte B)
        {
            return (B >> 4) * 10 + (B & 0xF);
        }

        private static byte ByteToBCD(int value)
        {
            return (byte)((value / 10 << 4) | (value % 10));
        }

        public static int DataSizeByte(this int wordLength)
        {
            return wordLength switch
            {
                1 => 1,
                2 => 1,
                3 => 1,
                4 => 2,
                6 => 4,
                5 => 2,
                7 => 4,
                8 => 4,
                28 => 2,
                29 => 2,
                _ => 0,
            };
        }

        public static bool GetBitAt(this byte[] buffer, int pos, int bit)
        {
            byte[] array = new byte[8] { 1, 2, 4, 8, 16, 32, 64, 128 };
            if (bit < 0)
            {
                bit = 0;
            }

            if (bit > 7)
            {
                bit = 7;
            }

            return (buffer[pos] & array[bit]) != 0;
        }

        public static void SetBitAt(this byte[] buffer, int pos, int bit, bool value)
        {
            byte[] array = new byte[8] { 1, 2, 4, 8, 16, 32, 64, 128 };
            if (bit < 0)
            {
                bit = 0;
            }

            if (bit > 7)
            {
                bit = 7;
            }

            if (value)
            {
                buffer[pos] = (byte)(buffer[pos] | array[bit]);
            }
            else
            {
                buffer[pos] = (byte)(buffer[pos] & ~array[bit]);
            }
        }

        public static int GetSIntAt(this byte[] buffer, int pos)
        {
            int num = buffer[pos];
            if (num < 128)
            {
                return num;
            }

            return num - 256;
        }

        public static void SetSIntAt(this byte[] buffer, int pos, int value)
        {
            if (value < -128)
            {
                value = -128;
            }

            if (value > 127)
            {
                value = 127;
            }

            buffer[pos] = (byte)value;
        }

        public static short GetIntAt(this byte[] buffer, int pos)
        {
            return (short)((buffer[pos] << 8) | buffer[pos + 1]);
        }

        public static void SetIntAt(this byte[] buffer, int pos, short value)
        {
            buffer[pos] = (byte)(value >> 8);
            buffer[pos + 1] = (byte)((uint)value & 0xFFu);
        }

        public static int GetDIntAt(this byte[] buffer, int pos)
        {
            return (((buffer[pos] << 8) + buffer[pos + 1] << 8) + buffer[pos + 2] << 8) + buffer[pos + 3];
        }

        public static void SetDIntAt(this byte[] buffer, int pos, int value)
        {
            buffer[pos + 3] = (byte)((uint)value & 0xFFu);
            buffer[pos + 2] = (byte)((uint)(value >> 8) & 0xFFu);
            buffer[pos + 1] = (byte)((uint)(value >> 16) & 0xFFu);
            buffer[pos] = (byte)((uint)(value >> 24) & 0xFFu);
        }

        public static long GetLIntAt(this byte[] buffer, int pos)
        {
            return (long)(((((((((ulong)buffer[pos] << 8) + buffer[pos + 1] << 8) + buffer[pos + 2] << 8) + buffer[pos + 3] << 8) + buffer[pos + 4] << 8) + buffer[pos + 5] << 8) + buffer[pos + 6] << 8) + buffer[pos + 7]);
        }

        public static void SetLIntAt(this byte[] buffer, int pos, long value)
        {
            buffer[pos + 7] = (byte)(value & 0xFF);
            buffer[pos + 6] = (byte)((value >> 8) & 0xFF);
            buffer[pos + 5] = (byte)((value >> 16) & 0xFF);
            buffer[pos + 4] = (byte)((value >> 24) & 0xFF);
            buffer[pos + 3] = (byte)((value >> 32) & 0xFF);
            buffer[pos + 2] = (byte)((value >> 40) & 0xFF);
            buffer[pos + 1] = (byte)((value >> 48) & 0xFF);
            buffer[pos] = (byte)((value >> 56) & 0xFF);
        }

        public static byte GetUSIntAt(this byte[] buffer, int pos)
        {
            return buffer[pos];
        }

        public static void SetUSIntAt(this byte[] buffer, int pos, byte value)
        {
            buffer[pos] = value;
        }

        public static ushort GetUIntAt(this byte[] buffer, int pos)
        {
            return (ushort)((buffer[pos] << 8) | buffer[pos + 1]);
        }

        public static void SetUIntAt(this byte[] buffer, int pos, ushort value)
        {
            buffer[pos] = (byte)(value >> 8);
            buffer[pos + 1] = (byte)(value & 0xFFu);
        }

        public static uint GetUDIntAt(this byte[] buffer, int pos)
        {
            return (uint)((((((buffer[pos] << 8) | buffer[pos + 1]) << 8) | buffer[pos + 2]) << 8) | buffer[pos + 3]);
        }

        public static void SetUDIntAt(this byte[] buffer, int pos, uint value)
        {
            buffer[pos + 3] = (byte)(value & 0xFFu);
            buffer[pos + 2] = (byte)((value >> 8) & 0xFFu);
            buffer[pos + 1] = (byte)((value >> 16) & 0xFFu);
            buffer[pos] = (byte)((value >> 24) & 0xFFu);
        }

        public static ulong GetULIntAt(this byte[] buffer, int pos)
        {
            return ((((((((((((((ulong)buffer[pos] << 8) | buffer[pos + 1]) << 8) | buffer[pos + 2]) << 8) | buffer[pos + 3]) << 8) | buffer[pos + 4]) << 8) | buffer[pos + 5]) << 8) | buffer[pos + 6]) << 8) | buffer[pos + 7];
        }

        public static void SetULintAt(this byte[] buffer, int pos, ulong value)
        {
            buffer[pos + 7] = (byte)(value & 0xFF);
            buffer[pos + 6] = (byte)((value >> 8) & 0xFF);
            buffer[pos + 5] = (byte)((value >> 16) & 0xFF);
            buffer[pos + 4] = (byte)((value >> 24) & 0xFF);
            buffer[pos + 3] = (byte)((value >> 32) & 0xFF);
            buffer[pos + 2] = (byte)((value >> 40) & 0xFF);
            buffer[pos + 1] = (byte)((value >> 48) & 0xFF);
            buffer[pos] = (byte)((value >> 56) & 0xFF);
        }

        public static byte GetByteAt(this byte[] buffer, int pos)
        {
            return buffer[pos];
        }

        public static void SetByteAt(this byte[] buffer, int pos, byte value)
        {
            buffer[pos] = value;
        }

        public static ushort GetWordAt(this byte[] buffer, int pos)
        {
            return buffer.GetUIntAt(pos);
        }

        public static void SetWordAt(this byte[] buffer, int pos, ushort value)
        {
            buffer.SetUIntAt(pos, value);
        }

        public static uint GetDWordAt(this byte[] buffer, int pos)
        {
            return buffer.GetUDIntAt(pos);
        }

        public static void SetDWordAt(this byte[] buffer, int pos, uint value)
        {
            buffer.SetUDIntAt(pos, value);
        }

        public static ulong GetLWordAt(this byte[] buffer, int pos)
        {
            return buffer.GetULIntAt(pos);
        }

        public static void SetLWordAt(this byte[] buffer, int pos, ulong value)
        {
            buffer.SetULintAt(pos, value);
        }

        public static float GetRealAt(this byte[] buffer, int pos)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(buffer.GetUDIntAt(pos)), 0);
        }

        public static void SetRealAt(this byte[] buffer, int pos, float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            buffer[pos] = bytes[3];
            buffer[pos + 1] = bytes[2];
            buffer[pos + 2] = bytes[1];
            buffer[pos + 3] = bytes[0];
        }

        public static double GetLRealAt(this byte[] buffer, int pos)
        {
            return BitConverter.ToDouble(BitConverter.GetBytes(buffer.GetULIntAt(pos)), 0);
        }

        public static void SetLRealAt(this byte[] buffer, int pos, double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            buffer[pos] = bytes[7];
            buffer[pos + 1] = bytes[6];
            buffer[pos + 2] = bytes[5];
            buffer[pos + 3] = bytes[4];
            buffer[pos + 4] = bytes[3];
            buffer[pos + 5] = bytes[2];
            buffer[pos + 6] = bytes[1];
            buffer[pos + 7] = bytes[0];
        }

        public static DateTime GetDateTimeAt(this byte[] buffer, int pos)
        {
            int num = BCDtoByte(buffer[pos]);
            num = ((num >= 90) ? (num + 1900) : (num + 2000));
            int month = BCDtoByte(buffer[pos + 1]);
            int day = BCDtoByte(buffer[pos + 2]);
            int hour = BCDtoByte(buffer[pos + 3]);
            int minute = BCDtoByte(buffer[pos + 4]);
            int second = BCDtoByte(buffer[pos + 5]);
            int millisecond = BCDtoByte(buffer[pos + 6]) * 10 + BCDtoByte(buffer[pos + 7]) / 10;
            try
            {
                return new DateTime(num, month, day, hour, minute, second, millisecond);
            }
            catch (ArgumentOutOfRangeException)
            {
                return new DateTime(0L);
            }
        }

        public static void SetDateTimeAt(this byte[] buffer, int pos, DateTime value)
        {
            int num = value.Year;
            int month = value.Month;
            int day = value.Day;
            int hour = value.Hour;
            int minute = value.Minute;
            int second = value.Second;
            int num2 = (int)(value.DayOfWeek + 1);
            int value2 = value.Millisecond / 10;
            int num3 = value.Millisecond % 10;
            if (num > 1999)
            {
                num -= 2000;
            }

            buffer[pos] = ByteToBCD(num);
            buffer[pos + 1] = ByteToBCD(month);
            buffer[pos + 2] = ByteToBCD(day);
            buffer[pos + 3] = ByteToBCD(hour);
            buffer[pos + 4] = ByteToBCD(minute);
            buffer[pos + 5] = ByteToBCD(second);
            buffer[pos + 6] = ByteToBCD(value2);
            buffer[pos + 7] = ByteToBCD(num3 * 10 + num2);
        }

        public static DateTime GetDateAt(this byte[] buffer, int pos)
        {
            try
            {
                return new DateTime(1990, 1, 1).AddDays(buffer.GetIntAt(pos));
            }
            catch (ArgumentOutOfRangeException)
            {
                return new DateTime(0L);
            }
        }

        public static void SetDateAt(this byte[] buffer, int pos, DateTime value)
        {
            buffer.SetIntAt(pos, (short)(value - new DateTime(1990, 1, 1)).Days);
        }

        [Obsolete("Use GetTODAsDateTimeAt or GetTODAsTimeSpanAt instead")]
        public static DateTime GetTODAt(this byte[] buffer, int pos)
        {
            return buffer.GetTODAsDateTimeAt(pos);
        }

        public static DateTime GetTODAsDateTimeAt(this byte[] buffer, int pos)
        {
            try
            {
                return new DateTime(0L).AddMilliseconds(buffer.GetDIntAt(pos));
            }
            catch (ArgumentOutOfRangeException)
            {
                return new DateTime(0L);
            }
        }

        public static void SetTODAt(this byte[] buffer, int pos, DateTime value)
        {
            buffer.SetDIntAt(pos, (int)Math.Round(value.TimeOfDay.TotalMilliseconds));
        }

        public static TimeSpan GetTODAsTimeSpanAt(this byte[] buffer, int pos)
        {
            try
            {
                return TimeSpan.FromMilliseconds(buffer.GetDIntAt(pos));
            }
            catch (ArgumentOutOfRangeException)
            {
                return TimeSpan.Zero;
            }
        }

        public static void SetTODAt(this byte[] buffer, int pos, TimeSpan value)
        {
            buffer.SetDIntAt(pos, (int)Math.Round(value.TotalMilliseconds));
        }

        [Obsolete("Use GetLTODAsDateTimeAt or GetLTODAsTimeSpanAt instead")]
        public static DateTime GetLTODAt(this byte[] buffer, int pos)
        {
            return buffer.GetLTODAsDateTimeAt(pos);
        }

        public static DateTime GetLTODAsDateTimeAt(this byte[] buffer, int pos)
        {
            try
            {
                return new DateTime(Math.Abs(buffer.GetLIntAt(pos) / 100));
            }
            catch (ArgumentOutOfRangeException)
            {
                return new DateTime(0L);
            }
        }

        public static void SetLTODAt(this byte[] buffer, int pos, DateTime value)
        {
            buffer.SetLIntAt(pos, value.TimeOfDay.Ticks * 100);
        }

        public static TimeSpan GetLTODAsTimeSpanAt(this byte[] buffer, int pos)
        {
            try
            {
                return TimeSpan.FromTicks(Math.Abs(buffer.GetLIntAt(pos) / 100));
            }
            catch (ArgumentOutOfRangeException)
            {
                return TimeSpan.Zero;
            }
        }

        public static void SetLTODAt(this byte[] buffer, int pos, TimeSpan value)
        {
            buffer.SetLIntAt(pos, value.Ticks * 100);
        }

        public static DateTime GetLDTAt(this byte[] buffer, int pos)
        {
            try
            {
                return new DateTime(buffer.GetLIntAt(pos) / 100 + bias);
            }
            catch (ArgumentOutOfRangeException)
            {
                return new DateTime(0L);
            }
        }

        public static void SetLDTAt(this byte[] buffer, int pos, DateTime value)
        {
            buffer.SetLIntAt(pos, (value.Ticks - bias) * 100);
        }

        public static DateTime GetDTLAt(this byte[] buffer, int pos)
        {
            int year = buffer[pos] * 256 + buffer[pos + 1];
            int month = buffer[pos + 2];
            int day = buffer[pos + 3];
            int hour = buffer[pos + 5];
            int minute = buffer[pos + 6];
            int second = buffer[pos + 7];
            int millisecond = (int)buffer.GetUDIntAt(pos + 8) / 1000000;
            try
            {
                return new DateTime(year, month, day, hour, minute, second, millisecond);
            }
            catch (ArgumentOutOfRangeException)
            {
                return new DateTime(0L);
            }
        }

        public static void SetDTLAt(this byte[] buffer, int pos, DateTime value)
        {
            short value2 = (short)value.Year;
            byte b = (byte)value.Month;
            byte b2 = (byte)value.Day;
            byte b3 = (byte)value.Hour;
            byte b4 = (byte)value.Minute;
            byte b5 = (byte)value.Second;
            byte b6 = (byte)(value.DayOfWeek + 1);
            int value3 = value.Millisecond * 1000000;
            byte[] bytes = BitConverter.GetBytes(value2);
            buffer[pos] = bytes[1];
            buffer[pos + 1] = bytes[0];
            buffer[pos + 2] = b;
            buffer[pos + 3] = b2;
            buffer[pos + 4] = b6;
            buffer[pos + 5] = b3;
            buffer[pos + 6] = b4;
            buffer[pos + 7] = b5;
            buffer.SetDIntAt(pos + 8, value3);
        }

        public static string GetStringAt(this byte[] buffer, int pos)
        {
            int count = buffer[pos + 1];
            return Encoding.UTF8.GetString(buffer, pos + 2, count);
        }

        public static void SetStringAt(this byte[] buffer, int pos, int MaxLen, string value)
        {
            int num = value.Length;
            if (num > MaxLen)
            {
                num = MaxLen;
            }

            buffer[pos] = (byte)MaxLen;
            buffer[pos + 1] = (byte)num;
            Encoding.UTF8.GetBytes(value, 0, num, buffer, pos + 2);
        }

        public static string GetCharsAt(this byte[] buffer, int pos, int Size)
        {
            return Encoding.UTF8.GetString(buffer, pos, Size);
        }

        public static void SetCharsAt(this byte[] buffer, int pos, string value)
        {
            int num = buffer.Length - pos;
            if (num > value.Length)
            {
                num = value.Length;
            }

            Encoding.UTF8.GetBytes(value, 0, num, buffer, pos);
        }

        public static int GetCounter(this ushort value)
        {
            return BCDtoByte((byte)value) * 100 + BCDtoByte((byte)(value >> 8));
        }

        public static int GetCounterAt(this ushort[] buffer, int Index)
        {
            return buffer[Index].GetCounter();
        }

        public static ushort ToCounter(this int value)
        {
            return (ushort)(ByteToBCD(value / 100) + (ByteToBCD(value % 100) << 8));
        }

        public static void SetCounterAt(this ushort[] buffer, int pos, int value)
        {
            buffer[pos] = value.ToCounter();
        }

        //HOT noch was machen
        //public static S7Timer GetS7TimerAt(this byte[] buffer, int pos)
        //{
        //    return new S7Timer(new List<byte>(buffer).GetRange(pos, 12).ToArray());
        //}

        public static void SetS7TimespanAt(this byte[] buffer, int pos, TimeSpan value)
        {
            buffer.SetDIntAt(pos, (int)value.TotalMilliseconds);
        }

        public static TimeSpan GetS7TimespanAt(this byte[] buffer, int pos)
        {
            if (buffer.Length < pos + 4)
            {
                return default(TimeSpan);
            }

            int num = buffer[pos];
            num <<= 8;
            num += buffer[pos + 1];
            num <<= 8;
            num += buffer[pos + 2];
            num <<= 8;
            num += buffer[pos + 3];
            return new TimeSpan(0, 0, 0, 0, num);
        }
    }
}
