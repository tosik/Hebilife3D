namespace Legacy.Hebilife
{
    public class Snake
    {
        // 体の長さ
        public int length;

        // 体
        public int[] body_x = new int[100];
        public int[] body_y = new int[100];

        public int[] pre_body_x = new int[100];
        public int[] pre_body_y = new int[100];

        public bool is_dead;

        // 全てのSnakeはFieldを参照する
        public Field field;

        // 動く方向
        public int direction_x;
        public int direction_y;

        // オブジェクトに割り当てられるユニークな番号
        public int id;

        public Brain brain = new Brain();

        public Snake(int id, Field field)
        {
            this.id = id;
            this.field = field;

            is_dead = false;

            // 動く方向の初期化
            int r = field.random(4);
            switch (r)
            {
                case 0:
                    direction_x = 0;
                    direction_y = 1;
                    break;
                case 1:
                    direction_x = 1;
                    direction_y = 0;
                    break;
                case 2:
                    direction_x = 0;
                    direction_y = -1;
                    break;
                case 3:
                    direction_x = -1;
                    direction_y = 0;
                    break;
                default:
                    break;
            }

            // 座標の初期化
            length = field.random(field.length_max - 1) + 1;
            body_x[0] = field.random(Field.size_x - length);
            body_y[0] = field.random(Field.size_y - length);
            for (int i = 1; i < length; i++)
            {
                body_x[i] = body_x[i - 1] - direction_x;
                body_y[i] = body_y[i - 1] - direction_y;
            }
            for (int i = length; i < 100; i++)
            {
                body_x[i] = -1;
                body_y[i] = -1;
            }
        }

        public Snake(int id, Field field, Snake origin)
        {
            this.id = id;
            this.field = field;

            is_dead = false;

            length = origin.length / 2;
            for (int i = 0; i < length; i++)
            {
                body_x[i] = origin.pre_body_x[i + length];
                body_y[i] = origin.pre_body_y[i + length];
                pre_body_x[i] = body_x[i];
                pre_body_y[i] = body_y[i];
            }
            for (int i = length; i < 100; i++)
            {
                body_x[i] = -1;
                body_y[i] = -1;
            }

            // 動く方向の初期化
            direction_x = origin.pre_body_x[length - 1] - origin.pre_body_x[length];
            direction_y = origin.pre_body_y[length - 1] - origin.pre_body_y[length];

            brain.copy(origin.brain);
        }

        // stepする前に遺伝子により行動を決定し、仮に行動する
        public void pre_move()
        {
            if (is_dead)
                return;

            // bodyをpre_bodyにコピー
            for (int i = 0; i < length; i++)
            {
                pre_body_x[i] = body_x[i];
                pre_body_y[i] = body_y[i];
            }

            // 動く方向を決定
            int x = body_x[0];
            int y = body_y[0];
            int[] input = new int[NN.N_NEURON];

            // いじって遊ぶ場所
            input[0] = field.getHazard(x + direction_y, y - direction_x) ? 1 : 0;
            input[1] = field.getHazard(x + direction_x * 3, y + direction_y * 3) ? 1 : 0;
            input[2] = field.getHazard(x + direction_y, y + direction_x) ? 1 : 0;
            int output = brain.input_and_output(input);

            if (output == 1)
            {
                int temp = direction_x;
                direction_x = direction_y;
                direction_y = -temp;
            }
            else if (output == 2)
            {
                int temp = direction_x;
                direction_x = -direction_y;
                direction_y = temp;
            }


            // その方向に動く
            for (int i = length; i > 0; i--)
            {
                pre_body_x[i] = pre_body_x[i - 1];
                pre_body_y[i] = pre_body_y[i - 1];
            }
            pre_body_x[0] += direction_x;
            pre_body_y[0] += direction_y;

            // wrap
            if (pre_body_x[0] == -1)
                pre_body_x[0] = Field.size_x - 1;
            if (pre_body_y[0] == -1)
                pre_body_y[0] = Field.size_y - 1;
            if (pre_body_x[0] == Field.size_x)
                pre_body_x[0] = 0;
            if (pre_body_y[0] == Field.size_y)
                pre_body_y[0] = 0;

            //    if ( output == 3 && length >= field.length_max )
            //        segmentation();
        }

        public void step()
        {
            if (is_dead)
                return;

            if (field.isDead(id))
            {
                die();
                return;
            }
            else if (field.canEat(id))
            {
                eat();
                if (length >= field.length_max)
                    separate();
            }
            move();
        }

        // pre_move で決定した行動を実際に行う
        public void move()
        {
            if (is_dead)
                return;

            for (int i = 0; i < length; i++)
            {
                body_x[i] = pre_body_x[i];
                body_y[i] = pre_body_y[i];
            }
        }

        public void die()
        {
            is_dead = true;

            for (int i = 0; i < length; i++)
            {
                field.foods[body_x[i], body_y[i]] = true;
            }
        }

        public void eat()
        {
            length++;
            if (length == 100)
                length--;
            field.foods[pre_body_x[0], pre_body_y[0]] = false;
        }

        public void separate()
        {
            field.makeSnake(this);
            length /= 2;
        }
    }
}