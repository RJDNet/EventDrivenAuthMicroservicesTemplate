import pika

topics = {'csharpmicroservice.test1','csharpmicroservice.test2'}
exchangeName = 'micro_exchange'
queueName = 'rpc_queue'

connection = pika.BlockingConnection(pika.ConnectionParameters(
                                        host = 'messagebroker', 
                                        port = 5672, 
                                        credentials = pika.PlainCredentials('admin', 'admin')
                                    ))

channel = connection.channel()

channel.exchange_declare(exchange=exchangeName, exchange_type='topic', durable=False)

result = channel.queue_declare('', exclusive=False, durable=False)

for binding_key in topics:
    channel.queue_bind(
        exchange=exchangeName, queue=result.method.queue, routing_key=binding_key)

print('PythonMicroservice awaiting message...')

def callback(ch, method, properties, body):
    print('PythonMicroservice recieved message: %r' % body.decode('utf-8'))

channel.basic_consume(
    queue=result.method.queue, on_message_callback=callback, auto_ack=True)

channel.start_consuming()