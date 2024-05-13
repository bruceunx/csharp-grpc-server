import asyncio
import logging

import grpc
import greet_pb2
import greet_pb2_grpc


class Greeter(greet_pb2_grpc.GreeterServicer):

    def SayHello(self, request, context):
        return greet_pb2.HelloReply(message="Hello, %s!" % request.name)

    async def Greeting(self, request, context):
        for i in range(10):
            yield greet_pb2.HelloReply(
                message=f"Hello number {i}, {request.name}!")
            await asyncio.sleep(1)


async def serve() -> None:
    server = grpc.aio.server()
    greet_pb2_grpc.add_GreeterServicer_to_server(Greeter(), server)
    listen_addr = "[::]:5214"
    server.add_insecure_port(listen_addr)
    logging.info("Starting server on %s", listen_addr)
    await server.start()
    await server.wait_for_termination()


if __name__ == "__main__":
    logging.basicConfig(level=logging.INFO)
    asyncio.run(serve())
