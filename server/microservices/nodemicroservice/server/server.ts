var amqpConnect = require('amqplib/callback_api');
var amqpCredentials = require('amqplib');

interface Options {
  credentials: string;
}

const topics: string[] = ['csharpmicroservice.test1', 'csharpmicroservice.test2'];
const exchangeName: string = 'micro_exchange';
const queueName: string = 'rpc_queue';
const host: string = 'messagebroker';

const opt: Options = { credentials: amqpCredentials.credentials.plain('admin', 'admin') };

amqpConnect.connect(`amqp://${host}`, opt, function(error0, connection) {
  if (error0) {
    throw error0;
  }

  connection.createChannel(function(error1, channel) {
    if (error1) {
      throw error1;
    }
    
    channel.assertExchange(exchangeName, 'topic', {
      durable: false
    });

    channel.assertQueue('', {
      exclusive: false,
      durable: false
    }, function(error2, q) {
      if (error2) {
        throw error2;
      }

      console.log('NodeMicroservice awaiting message...');

      topics.forEach(function(key) {
        channel.bindQueue(q.queueName, exchangeName, key);
      });

      channel.consume(q.queueName, function(msg) {
        console.log("NodeMicroservice received message: " + msg.content.toString());
      }, {
        noAck: true
      });
    });
  });
});