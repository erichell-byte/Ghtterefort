namespace CardProject
{
	public interface ISaver<T>
	{
		void Save(T param);
		T Load();
	}
}