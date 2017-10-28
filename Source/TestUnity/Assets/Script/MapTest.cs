using Nelibur.ObjectMapper;
using UnityEngine;

namespace Assets.Script
{
	public class MapTest : MonoBehaviour
	{
		private void Start()
		{
			var testModel = new TestModel
			{
				Id = 1,
				Name = "haha"
			};
			TinyMapper.Bind<TestModel, TestDto>();
			var dto = TinyMapper.Map<TestDto>(testModel);
			Debug.LogFormat("TestModel Id[{0}] Name[{1}] => TestDto Id[{2}] Name[{3}]", testModel.Id, testModel.Name, dto.Id, dto.Name);

			var testStaticModel = new TestStaticModel();
			TinyMapper.Bind<TestStaticModel, TestDto>();
			var testDto = TinyMapper.Map<TestDto>(testStaticModel);
			Debug.LogFormat("TestStaticModel Id[{0}] Name[{1}] => TestDto Id[{2}] Name[{3}]", TestStaticModel.Id, TestStaticModel.Name, testDto.Id, testDto.Name);
		}

		private void Update()
		{

		}
	}

	public class TestModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class TestStaticModel
	{
		public static int Id = 1;
		public static string Name = "test";
	}

	public class TestDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}