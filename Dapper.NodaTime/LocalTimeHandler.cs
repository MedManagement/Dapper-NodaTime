﻿using System;
using System.Data;
using System.Data.SqlClient;
using NodaTime;

namespace Dapper.NodaTime
{
    public class LocalTimeHandler : SqlMapper.TypeHandler<LocalTime>
    {
        protected LocalTimeHandler()
        {
        }

        public static readonly LocalTimeHandler Default = new LocalTimeHandler();

        public override void SetValue(IDbDataParameter parameter, LocalTime value)
        {
            //parameter.Value = TimeSpan.FromTicks(value.TickOfDay);
            parameter.Value = new DateTime(1753, 1, 1, value.Hour, value.Minute, value.Second,0, DateTimeKind.Unspecified);

            var sqlParameter = parameter as SqlParameter;
            if (sqlParameter != null)
            {
                sqlParameter.SqlDbType = SqlDbType.DateTime;
            }
        }

        public override LocalTime Parse(object value)
        {
            if (value is TimeSpan)
            {
                var ts = (TimeSpan) value;
                return LocalTime.FromTicksSinceMidnight(ts.Ticks);
            }

            if (value is DateTime)
            {
                var dt = (DateTime) value;
                return LocalTime.FromTicksSinceMidnight(dt.TimeOfDay.Ticks);
            }

            throw new DataException("Cannot convert " + value.GetType() + " to NodaTime.LocalTime");
        }
    }
}
