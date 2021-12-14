

/**
 * Copyright (c) blueback
 * Released under the MIT License
 * @brief プールリスト。
*/


/** BlueBack.PoolList
*/
namespace BlueBack.PoolList
{
	/** BufferList_IndexToItem
	*/
	public struct BufferList_IndexToItem<ITEM>
		where ITEM : BufferList_Item_Base
	{
		/** list
		*/
		public System.Collections.Generic.LinkedListNode<ITEM>[] list;

		/** constructor
		*/
		public BufferList_IndexToItem(System.Collections.Generic.LinkedList<ITEM> a_list)
		{
			//list
			this.list = new System.Collections.Generic.LinkedListNode<ITEM>[a_list.Count];
			System.Collections.Generic.LinkedListNode<ITEM> t_node = a_list.First;
			while(t_node != null){
				this.list[t_node.Value.GetIndex()] = t_node;
				t_node = t_node.Next;
			}
		}

		/** a_listに所属しているノードを検索。 a_index から 後方へ。
		*/
		public int FindInListIndexToEnd(System.Collections.Generic.LinkedList<ITEM> a_list,int a_index)
		{
			int ii_max = this.list.Length;
			for(int ii = a_index;ii<ii_max;ii++){
				if(this.list[ii].List == a_list){
					return ii;
				}
			}
			return -1;
		}

		/** a_listに所属しているノードを検索。 a_index から 前方へ。
		*/
		public int FindInListIndexToStart(System.Collections.Generic.LinkedList<ITEM> a_list,int a_index)
		{
			for(int ii=a_index;ii>=0;ii--){
				if(this.list[ii].List == a_list){
					return ii;
				}
			}
			return -1;
		}
	}
}

