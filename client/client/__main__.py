import grpc
import greet_pb2
import greet_pb2_grpc


def run():
    with grpc.insecure_channel('localhost:5214') as channel:
        stub = greet_pb2_grpc.GreeterStub(channel)
        response = stub.SayHello(greet_pb2.HelloRequest(name='you'))
        print("Greeter client received: " + response.message)
        response = stub.Greeting(greet_pb2.HelloRequest(name='you'))
        for data in response:
            print("Greeter client received: " + data.message)


run()
