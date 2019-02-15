using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MiningGameManager : MonoBehaviour
{
    private enum Direction
    {
        North, South, West, East
    }

    public Transform gameTilePrefab;
    public Transform cartPrefab;
    public GameObject gameOverSprite;
    public bool isGamePlayed = true;

    public const int height = 9, width = 16;
    public GameTile[,] gameField = new GameTile[width, height]; // 1 if edge, 0 if blank, 2 if item.
    public List<Cart> carts = new List<Cart>();
    private Direction _direction = Direction.East;
    public int score = 0;
    public Vector2 direction { get
        {
            switch ((int)_direction)
            {
                case 0:
                    return new Vector2(0, 1);
                case 1:
                    return new Vector2(0, -1);
                case 2:
                    return new Vector3(-1, 0);
                case 3:
                    return new Vector2(1, 0);
                default:
                    throw new InvalidOperationException("EREOREJQAsdng");
            }
        }
    }


    private void Start()
    {
        InitGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            _direction = Direction.North;
        else if(Input.GetKeyDown(KeyCode.DownArrow))
            _direction = Direction.South;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            _direction = Direction.West;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            _direction = Direction.East;
    }

    public void InitGame()
    {
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                gameField[i, j] = Instantiate(gameTilePrefab, new Vector3(i, j, 0), Quaternion.identity).gameObject.GetComponent<GameTile>();
                if (i == 0 || j == 0 || i == width - 1|| j == height - 1)
                    gameField[i, j].state = 1;
                else
                    gameField[i, j].state = 0;
                gameField[i, j].SetSpriteOfState();
            }
        MakeCart(1, 4);
        StartCoroutine(_CartMovement());
        StartCoroutine(_IronProducing());
    }


    public void ProceedCarts()
    {

    }

    private void _MoveCarts()
    {
        for (int i = carts.Count - 1; i >= 0; --i)
        {
            Cart c = carts[i];
            gameField[c.x, c.y].isBodyOnIt = false;
            if (c.nextCart == null)
            {
                c.x += (int)direction.x;
                c.y += (int)direction.y;
            }
            else
            {
                c.x = c.nextCart.x;
                c.y = c.nextCart.y;
            }

            foreach(Cart cart in carts)
            {
                gameField[cart.x, cart.y].isBodyOnIt = true;
            }

            c.transform.position = new Vector3(c.x, c.y);
        }

        if (gameField[carts.First().x, carts.First().y].state == 2)
        {
            score++;
            gameField[carts.First().x, carts.First().y].state = 0;
            gameField[carts.First().x, carts.First().y].SetSpriteOfState();
            Vector2 toMake = new Vector2();
            if (carts.Count == 1)
            {
                toMake.x = carts[0].x - direction.x;
                toMake.y = carts[0].y - direction.y;
            }
            else
            {
                toMake.x = carts[carts.Count - 1].x - (carts[carts.Count - 2].x - carts[carts.Count - 1].x);
                toMake.y = carts[carts.Count - 1].y - (carts[carts.Count - 2].y - carts[carts.Count - 1].y);
            }
            MakeCart((int)toMake.x, (int)toMake.y);
        }

        if (carts.FindAll(c => c.x == carts.First().x && c.y == carts.First().y).Count > 1 || gameField[carts.First().x, carts.First().y].state == 1)
            GameOver();
    }

    private IEnumerator _CartMovement()
    {
        while(isGamePlayed)
        {
            _MoveCarts();
            yield return new WaitForSeconds(0.66f);
        }
    }
    private IEnumerator _IronProducing()
    {
        while(isGamePlayed)
        {
            MakeNewIron();
            yield return new WaitForSeconds(5f);
        }
    }

    public void MakeNewIron()
    {
        System.Random r = new System.Random();
        int x = r.Next() % (width - 2) + 1; // 1 <= x <= 30
        int y = r.Next() % (height - 2) + 1; // 1 <= y <= 15
        if (gameField[x, y].state != 0 || gameField[x, y].isBodyOnIt)
            MakeNewIron();
        else
        {
            gameField[x, y].state = 2;
            gameField[x, y].SetSpriteOfState();
            return;
        }
    }

    public void GameOver()
    {
        isGamePlayed = false;
        gameOverSprite.SetActive(true);
    }

    public void MakeCart(int x, int y)
    {
        Transform cart = Instantiate(cartPrefab, new Vector3(x, y), Quaternion.identity);
        cart.GetComponent<Cart>().x = x;
        cart.GetComponent<Cart>().y = y;
        if (carts.Count == 0)
            cart.gameObject.GetComponent<Cart>().nextCart = null;
        else
            cart.gameObject.GetComponent<Cart>().nextCart = carts[carts.Count - 1];
        carts.Add(cart.gameObject.GetComponent<Cart>());
        gameField[x, y].isBodyOnIt = true;
    }
}
