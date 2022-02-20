import tweepy
client = tweepy.Client(consumer_key="kxaVy9B88iKRgwmzsiBkTHY4V",
                       consumer_secret="MrlaBTbwfLKfOEE2DqodNWPJqEYK4lBKoH4eQwM9hNORgMn2VO",
                       access_token="996874516298166272-gTRPD0vHAdn2HhJ7m9d6JqVujB20To6",
                       access_token_secret="AyDJ72FMEJBJr9VPydEDTSqWSJFcQJQ8UAX8dLNPpjbGN",
                       bearer_token='AAAAAAAAAAAAAAAAAAAAAL2lZQEAAAAA2HdvxrbmgkVxQWXGXu6OIINIGec%3Dp5zSWarf5Vuxck9ODw3NQm1h3f0bbaNjccAyoC1dBT67ikUVSN')


def create_api():

    consumer_key = "kxaVy9B88iKRgwmzsiBkTHY4V"
    consumer_secret = "MrlaBTbwfLKfOEE2DqodNWPJqEYK4lBKoH4eQwM9hNORgMn2VO"
    access_token = "996874516298166272-gTRPD0vHAdn2HhJ7m9d6JqVujB20To6"
    access_token_secret = "AyDJ72FMEJBJr9VPydEDTSqWSJFcQJQ8UAX8dLNPpjbGN"

    auth = tweepy.OAuthHandler(consumer_key, consumer_secret)
    auth.set_access_token(access_token, access_token_secret)

    api = tweepy.API(auth, wait_on_rate_limit=True)

    try:
        api.verify_credentials()
        print("Authentication OK")
    except:
        print("Error during authentication")

    return api
