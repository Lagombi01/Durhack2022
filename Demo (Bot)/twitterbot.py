import tweepy
import requests
import time
import json
from datetime import datetime, timezone, timedelta
from boot import create_api
from PIL import Image
from boot import client


def read_Poll(poll_id):

    dictionary = {}
    url = "https://strawpoll.com/api/poll/" + poll_id
    results = requests.get(url).json()
    poll_info = results['content']
    poll_content = poll_info['poll']
    poll_answers = poll_content['poll_answers']

    for answer in poll_answers:
        athlete = answer['answer']
        votes = answer['votes']
        dictionary[athlete] = votes

    response = requests.post("http://127.0.0.1:3000/sentimental/poll", #json=json.dumps(dictionary),
                             headers={**dictionary,**{'API-KEY': client.consumer_key}})


def generate_Media(tweet):
    query = tweet.text
    values = query.split(", ")
    input_data = {
        "age": values[0],
        "height": values[1],
        "weight": values[2],
        "interval": values[3]
    }

    input = json.dumps(input_data)

    # send to API
    # media = requests.post("", json=query, headers={'API-KEY': client.consumer_key}
    #                      ).json()


def main():

    api = create_api()

    Confirmed = False
    list_of_names = []

    while not Confirmed:

        list_of_names = []
        number = ""
        while not number.isnumeric():
            number = input("How many athletes are being compared?")
            if not number.isnumeric():
                print("Hey, that wasn't a number lol try again.")

        for i in range(0, int(number)):
            name = input("Enter name of athlete")
            list_of_names.append(name)

        print(list_of_names)
        confirmation = input("Are these correct? (y/n)")
        if confirmation == "y":
            Confirmed = True

    poll_start_time = datetime.now(timezone.utc)
    poll_end_time = poll_start_time + timedelta(days=1)
    poll_data = {
        "poll": {
            "title": "Who do you think is going to win?",
            "answers": list_of_names,
            "priv": False,
            "co": False,
            "deadline": str(poll_end_time),
            "captcha": False
        }
    }

    poll = requests.post("https://strawpoll.com/api/poll", json=poll_data, headers={'API-KEY': client.consumer_key}
                         ).json()
    api.update_status(status="check out this epic poll: https://strawpoll.com/" + poll["content_id"])
    last_tweet = (api.user_timeline(count=1))[0]
    last_id = last_tweet.id

    while True:
        time.sleep(30)
        current_time = datetime.now(timezone.utc)
        mentions = api.search_tweets(q="@LSlayer2")

        for tweets in mentions:
            if tweets.id > last_id:
                # image = generate_Media(tweets)

                api.update_status_with_media(status="Here you go!", in_reply_to_status_id=tweets.id,
                                             auto_populate_reply_metadata=True, filename="Jum-P.png")
                last_id = tweets.id

        if current_time >= poll_end_time:
            read_Poll(poll["content_id"])


read_Poll("83gef2yre")
