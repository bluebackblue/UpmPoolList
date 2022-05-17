

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief プールリスト。
*/


/** BlueBack.PoolList
*/
namespace BlueBack.PoolList
{
	/** BufferList
	*/
	public sealed class BufferList<ITEM,BUFFER> where ITEM : BufferList_Item_Base
	{
		/** dirty
		*/
		private bool dirty;

		/** indextoitem
		*/
		private BufferList_IndexToItem<ITEM> indextoitem;

		/** list_free
		*/
		public System.Collections.Generic.LinkedList<ITEM> list_free;

		/** list_use
		*/
		public System.Collections.Generic.LinkedList<ITEM> list_use;

		/** buffer
		*/
		public BUFFER[] buffer;

		/** constructor
		*/
		public BufferList(BUFFER[] a_buffer,System.Func<ITEM> a_creater)
		{
			//dirty
			this.dirty = false;

			//list_free
			this.list_free = new System.Collections.Generic.LinkedList<ITEM>();
			int ii_max = a_buffer.Length;
			for(int ii=0;ii<ii_max;ii++){
				ITEM t_raw = a_creater();
				t_raw.SetIndex(a_buffer.Length - ii - 1);
				System.Collections.Generic.LinkedListNode<ITEM> t_node = new System.Collections.Generic.LinkedListNode<ITEM>(t_raw);
				this.list_free.AddLast(t_node);
			}

			//indextoitem
			this.indextoitem = new BufferList_IndexToItem<ITEM>(this.list_free);

			//use
			this.list_use = new System.Collections.Generic.LinkedList<ITEM>();

			//buffer
			this.buffer = a_buffer;
		}

		/** Create
		*/
		public System.Collections.Generic.LinkedListNode<ITEM> Create()
		{
			System.Collections.Generic.LinkedListNode<ITEM> t_node = this.list_free.Last;
			if(t_node != null){
				this.dirty = true;
				this.list_free.Remove(t_node);
				this.list_use.AddLast(t_node);
				return t_node;
			}
			return null;
		}

		/** Delete
		*/
		public void Delete(System.Collections.Generic.LinkedListNode<ITEM> a_item)
		{
			if(a_item != null){
				this.dirty = true;
				this.list_use.Remove(a_item);
				this.list_free.AddLast(a_item);
			}else{
				#if(DEF_AMZY_LIST_ASSERT)
				DebugTool.Assert(false,"null");
				#endif
			}
		}

		/** 隙間を埋める。バッファは入れ替えない。
		*/
		public void Gc()
		{
			if(this.dirty == true){
				this.dirty = false;
				int t_max = this.list_use.Count;
				int t_index_use = this.indextoitem.FindInListIndexToStart(this.list_use,this.indextoitem.list.Length - 1);
				int t_index_free = 0;
				while(t_index_use >= t_max){
					t_index_free = this.indextoitem.FindInListIndexToEnd(this.list_free,t_index_free);
					{
						System.Collections.Generic.LinkedListNode<ITEM> t_node_a = this.indextoitem.list[t_index_free];
						System.Collections.Generic.LinkedListNode<ITEM> t_node_b = this.indextoitem.list[t_index_use];
						int t_index_a = t_node_a.Value.GetIndex();
						int t_index_b = t_node_b.Value.GetIndex();
						t_node_a.Value.SetIndex(t_index_b);
						t_node_b.Value.SetIndex(t_index_a);
						this.indextoitem.list[t_index_b] = t_node_a;
						this.indextoitem.list[t_index_a] = t_node_b;
					}
					t_index_use = this.indextoitem.FindInListIndexToStart(this.list_use,t_index_use);
				}
			}
		}


		/** 隙間を埋める。バッファも入れ替える。
		*/
		public void GcWithSwapBuffer()
		{
			if(this.dirty == true){
				this.dirty = false;
				int t_max = this.list_use.Count;
				int t_index_use = this.indextoitem.FindInListIndexToStart(this.list_use,this.indextoitem.list.Length - 1);
				int t_index_free = 0;
				while(t_index_use >= t_max){
					t_index_free = this.indextoitem.FindInListIndexToEnd(this.list_free,t_index_free);
					{
						System.Collections.Generic.LinkedListNode<ITEM> t_node_a = this.indextoitem.list[t_index_free];
						System.Collections.Generic.LinkedListNode<ITEM> t_node_b = this.indextoitem.list[t_index_use];
						int t_index_a = t_node_a.Value.GetIndex();
						int t_index_b = t_node_b.Value.GetIndex();
						BUFFER t_temp = this.buffer[t_index_a];
						this.buffer[t_index_a] = this.buffer[t_index_b];
						this.buffer[t_index_b] = t_temp;
						t_node_a.Value.SetIndex(t_index_b);
						t_node_b.Value.SetIndex(t_index_a);
						this.indextoitem.list[t_index_b] = t_node_a;
						this.indextoitem.list[t_index_a] = t_node_b;
					}
					t_index_use = this.indextoitem.FindInListIndexToStart(this.list_use,t_index_use);
				}
			}
		}
	}
}

