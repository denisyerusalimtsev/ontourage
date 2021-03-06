﻿using Ontourage.Core.Entities;
using Ontourage.Core.Interfaces;
using System.Collections.Generic;
using System.Data;

namespace Ontourage.DataAccess.SqlServer
{
    public class DbHotelRepository : IHotelRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DbHotelRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<HotelAggregate> GetAllHotels()
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var hotels = new List<HotelAggregate>();

                IDbCommand command = connection.CreateCommand();
                command.CommandText =
                    "SELECT h.Id, h.HotelName, h.CountOfStars, c.Code AS CountryCode, c.Country AS CountryName " +
                    "FROM Hotels h " +
                    "INNER JOIN Countries c ON h.CountryCode = c.Code";
                IDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var hotel = ReadHotel(reader);
                    hotels.Add(hotel);
                }
                return hotels;
            }
        }
        public HotelAggregate GetHotelById(int id)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText =
                    "SELECT h.Id, h.HotelName, h.CountOfStars, c.Code AS CountryCode, c.Country AS CountryName " +
                    "FROM Hotels h " +
                    "INNER JOIN Countries c ON h.CountryCode = c.Code " +
                    "WHERE h.Id = @Id";
                

                command.AddParameter("@Id", id);

                IDataReader reader = command.ExecuteReader();
                return reader.Read() ? ReadHotel(reader) : null;
            }
        }

        public void AddHotel(Hotel hotel)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Hotels (HotelName, CountryCode, CountOfStars) " +
                                      "VALUES (@HotelName, @CountryCode, @CountOfStars)";

                command.AddParameter("@HotelName", hotel.HotelName);
                command.AddParameter("@CountryCode", hotel.CountryCode);
                command.AddParameter("@CountOfStars", hotel.CountOfStars);

                command.ExecuteNonQuery();
            }
        }

        public void EditHotel(Hotel hotel)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Hotels SET " +
                                      "HotelName = @HotelName, " +
                                      "CountryCode = @CountryCode, " +
                                      "CountOfStars = @CountOfStars " +
                                      "WHERE Id = @Id";

                command.AddParameter("@Id", hotel.Id);
                command.AddParameter("@HotelName", hotel.HotelName);
                command.AddParameter("@CountryCode", hotel.CountryCode);
                command.AddParameter("@CountOfStars", hotel.CountOfStars);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteHotel(int id)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Hotels " +
                                      "WHERE Id = @Id";

                command.AddParameter("@Id", id);

                command.ExecuteNonQuery();
            }
        }

        private HotelAggregate ReadHotel(IDataReader reader)
        {
            return new HotelAggregate(
                id: (int)reader["Id"],
                hotelName: reader["HotelName"].ToString(),
                country: new Country(
                    countryCode: reader["CountryCode"].ToString(),
                    countryName: reader["CountryName"].ToString()),
                countOfStars: (int)reader["CountOfStars"]);
        }
    }
}
