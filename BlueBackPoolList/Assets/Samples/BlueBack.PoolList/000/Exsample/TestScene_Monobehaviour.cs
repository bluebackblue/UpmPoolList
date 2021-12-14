

/** Samples.PoolList.Bgm
*/
namespace Samples.PoolList.Bgm
{
	/** TestScene_Monobehaviour
	*/
	public sealed class TestScene_Monobehaviour : UnityEngine.MonoBehaviour
	{
		/** debugview
		*/
		public System.Collections.Generic.List<string> debugview;

		/** Item
		*/
		public class Item : BlueBack.PoolList.BufferList_Item_Base
		{
			/** index
			*/
			public int index;

			/** life
			*/
			public int life;

			/** GetIndex
			*/
			public int GetIndex()
			{
				return this.index;
			}

			/** SetIndex
			*/
			public void SetIndex(int a_index)
			{
				this.index = a_index;
			}
		}

		/** Buffer
		*/
		public struct Buffer
		{
			public int createtime;
			public int data;
		}

		/** list
		*/
		private BlueBack.PoolList.BufferList<Item,Buffer> list;
		private Buffer[] buffer;

		/** createtime
		*/
		private int createtime;

		/** Awake
		*/
		public void Awake()
		{
			//debugview
			this.debugview = new System.Collections.Generic.List<string>();

			//buffer
			this.buffer = new Buffer[30];
			for(int ii=0;ii<this.buffer.Length;ii++){
			}

			//list
			this.list = new BlueBack.PoolList.BufferList<Item,Buffer>(this.buffer,()=>{return new Item();});

			//createtime
			this.createtime = 0;
		}

		/** FixedUpdate
		*/
		public void FixedUpdate()
		{
			//�V�K�쐬�B
			if(UnityEngine.Random.value >= 0.995f){
				if(this.list.list_use.Count < this.list.list_free.Count){
					System.Collections.Generic.LinkedListNode<Item> t_node = this.list.Create();
					t_node.Value.life = (int)(UnityEngine.Random.value * 1000 + 1000);

					this.createtime++;
					this.buffer[t_node.Value.index].createtime = this.createtime;
				}
			}

			//�X�V�B
			{
				System.Collections.Generic.LinkedListNode<Item> t_node = this.list.list_use.First;
				while(t_node != null){
					System.Collections.Generic.LinkedListNode<Item> t_node_next = t_node.Next;

					//�A�C�e���̍X�V�B
					t_node.Value.life--;

					if(t_node.Value.life <= 0){
						this.list.Delete(t_node);
					}else{
						//�o�b�t�@�̍X�V�B
						this.list.buffer[t_node.Value.index].data = t_node.Value.life;
					}
					t_node = t_node_next;
				}
			}

			//���Ԃ𖄂߂�B�o�b�t�@������ւ���B
			this.list.GcWithSwapBuffer();

			//debugview�̍X�V�B
			{
				this.debugview.Clear();
				for(int ii=0;ii<this.list.list_use.Count;ii++){
					this.debugview.Add(this.list.buffer[ii].createtime.ToString() + " : " + this.list.buffer[ii].data.ToString());
				}
			}
		}
	}
}

