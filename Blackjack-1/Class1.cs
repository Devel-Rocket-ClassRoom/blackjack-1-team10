using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
//카드 정보를 담는 구조체
public struct Card
{
    public string shape; //카드 문양
    public string number; //카드 숫자
    public int value; //카드 값

    public override string ToString()
    {
        return $"[{shape + number}]";
    }
}

public class BlackJack
{
    private Card[] deck; //전체 카드 덱
    private Card[] player_hand; //플레이어 카드 덱
    private Card[] dealer_hand;  //딜러 카드 덱

    private int player_hand_count;
    private int dealer_hand_count;
    private int count;
    private bool flag;
    private bool IsPlayerAlive;
    private bool IsDealerAilve;

    public BlackJack()
    {

        string[] shape = { "♠", "♥", "♣", "◆" };
        int[] value = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        deck = new Card[shape.Length * value.Length];

        for (int i = 0; i < shape.Length; i++)
        {
            for (int j = 0; j < value.Length; j++)
            {
                int index = i * value.Length + j;
                deck[index].shape = shape[i];
                deck[index].value = value[j];
                if (value[j] == 1)
                {
                    deck[index].number = "A";
                }

                else if (value[j] == 11)
                {
                    deck[index].number = "J";
                }
                else if (value[j] == 12)
                {
                    deck[index].number = "Q";
                }
                else if (value[j] == 13)
                {
                    deck[index].number = "K";
                }
                else
                {
                    deck[index].number = Convert.ToString(value[j]);
                }
            }

        }

    }

    public void Shuffle()
    {
        Random rnd = new Random();
        for (int i = 0; i < deck.Length; i++)
        {
            int index = rnd.Next(0, deck.Length);
            Card temp = deck[i];
            deck[i] = deck[index];
            deck[index] = temp;
        }
    }

    public void PlayerInfo()
    {
        Console.Write($"플레이어의 패: ");
        for (int i = 0; i < player_hand_count; i++)
        {
            Console.Write($"{player_hand[i]} ");
        }
        Console.WriteLine($"\n플레이어의 점수: {Calculate(player_hand, player_hand_count)}");
    }

    public void DealerInfo()
    {
        if (!flag)
        {
            Console.WriteLine($"딜러의 패: [??] {dealer_hand[1]}");
            Console.WriteLine($"딜러의 점수: ?");
        }
        else
        {
            Console.Write($"딜러의 패: ");

            for (int i = 0; i < dealer_hand_count; i++)
            {
                Console.Write($"{dealer_hand[i]} ");
            }
            Console.WriteLine($"\n딜러의 점수: {Calculate(dealer_hand, dealer_hand_count)}");
        }
    }

    public int Calculate(Card[] card, int handcount)
    {
        int acount = 0;
        int sum = 0;
        for (int i = 0; i < handcount; i++)
        {
            if (card[i].value == 1)
            {
                sum += 11;
                acount++;
            }
            else if (card[i].value == 11 || card[i].value == 12 || card[i].value == 13)
            {
                sum += 10;
            }
            else
            {
                sum += card[i].value;
            }
        }

        for (int i = 0; i < acount; i++)
        {
            if (sum > 21)
            {
                sum -= 10;
            }
            else
            {
                break;
            }
        }
        return sum;
    }

    public void GetCard(ref Card[] card, ref int handcount)
    {
        card[handcount++] = deck[count++];
    }

    public void Start()
    {
        while (true)
        {
            flag = false;
            IsPlayerAlive = true;
            IsDealerAilve = true;
            player_hand_count = 0;
            dealer_hand_count = 0;
            count = 0;

            player_hand = new Card[10];
            dealer_hand = new Card[10];


            Shuffle();
            GetCard(ref dealer_hand, ref dealer_hand_count);
            GetCard(ref dealer_hand, ref dealer_hand_count);
            GetCard(ref player_hand, ref player_hand_count);
            GetCard(ref player_hand, ref player_hand_count);

            DealerInfo();
            PlayerInfo();

            while (true)
            {
                flag = true;
                Console.Write($"H(Hit) 또는 S(Stand)를 선택하세요: ");
                var result = Console.ReadLine();
                if (result == "H" || result == "h")
                {
                    GetCard(ref player_hand, ref player_hand_count);
                    Console.WriteLine($"플레이어가 카드를 받았습니다: {player_hand[player_hand_count - 1]}");
                    PlayerInfo();
                    if (Calculate(player_hand, player_hand_count) > 21)
                    {
                        Console.WriteLine($"버스트! 21을 초과했습니다.");
                        IsPlayerAlive = false;
                        break;
                    }

                }
                else if (result == "S" || result == "s")
                {
                    Console.WriteLine("플레이어가 Stand를 선택했습니다.");
                    break;
                }
            }

            if (IsPlayerAlive)
            {
                Console.WriteLine($"딜러의 숨겨진 카드: {dealer_hand[0]}");
                DealerInfo();
                while (Calculate(dealer_hand, dealer_hand_count) < 17)
                {
                    GetCard(ref dealer_hand, ref dealer_hand_count);
                    Console.WriteLine($"딜러가 카드를 받습니다: {dealer_hand[dealer_hand_count - 1]}");
                    DealerInfo();
                }
                if (Calculate(dealer_hand, dealer_hand_count) > 21)
                {
                    Console.WriteLine("딜러 버스트!");
                    IsDealerAilve = false;
                }
            }
            if (IsPlayerAlive && IsDealerAilve)
            {
                int playecrScore = Calculate(player_hand, player_hand_count);
                int dealerScore = Calculate(dealer_hand, dealer_hand_count);

                if (playecrScore > dealerScore)
                {
                    IsDealerAilve = false;
                }
                else if(playecrScore < dealerScore)
                {
                    IsPlayerAlive = false;
                }
                else
                {
                    IsPlayerAlive = false;
                    IsDealerAilve = false;
                }
            }

            Console.WriteLine("=== 게임 결과 ===");
            Console.WriteLine($"플레이어: {Calculate(player_hand, player_hand_count)}점");
            Console.WriteLine($"딜러: {Calculate(dealer_hand, dealer_hand_count)}점");

            if (IsPlayerAlive)
            {
                Console.WriteLine("플레이어 승리!");
            }
            else if (IsDealerAilve)
            {
                Console.WriteLine("플레이어 패배!");
            }
            else if (!IsPlayerAlive && !IsDealerAilve)
            {
                Console.WriteLine("무승부!");
            }


            Console.Write("새 게임을 하시겠습니까? (Y/N): ");
            var result2 = Console.ReadLine();
            if (result2 =="Y" || result2 == "y")
            {
                continue;
            }
            else
            {
                Console.WriteLine("게임을 종료합니다.");
                break;
            }
        }
    }
}