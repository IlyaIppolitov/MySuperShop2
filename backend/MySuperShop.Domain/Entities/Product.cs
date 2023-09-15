using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MySuperShop.Domain.Entities
{

	/// <summary>
	/// Модель данных для товара в магазине
	/// </summary>
	public class Product : ICloneable, IEntity
	{

		/// <summary>
		/// Конструтор
		/// </summary>
		/// <param name="name"></param> Название товара
		/// <param name="price"></param> Цена
		public Product(string name, decimal price)
		{
			// Валидация параметров
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
			if (price < 0) throw new ArgumentOutOfRangeException(nameof(price));

			Name = name;
			Price = price;
        }
        public Product(Guid id, string name, decimal price)
        {
            // Валидация параметров
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            if (price < 0) throw new ArgumentOutOfRangeException(nameof(price));

			Id = id;
            Name = name;
            Price = price;
        }

        [JsonConstructor]
		public Product(Guid id, string name, string? description, decimal price, double? stock, string? pic)
		{
			Id = id;
			Name = name;
			Pic = pic ?? "";
			Description = description ?? "Описание " + name;
			Price = price;
			Stock = stock ?? 0;
		}

		/// <summary> ID товара </
		[Key]
		public Guid Id { get; init; }

		/// <summary> Название товара </summary>
		public string Name { get; set; }

		/// <summary> Описание </summary>
		public string? Description { get; set; } = "";

		/// <summary> Цена </summary>
		public decimal Price { get; set; } = 0;

		/// <summary> Количество товара на складе </summary>
		public double? Stock { get; set; } = 0;

		/// <summary> Название товара </summary>
		public string? Pic { get; set; } = "";

		/// <summary> Метод для реализации интерфейса клонирования </summary>
		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}