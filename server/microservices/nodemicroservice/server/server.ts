#!/usr/bin/env node
var amqp = require('amqplib/callback_api');

const topics = ['csharpmicroservice.test1', 'csharpmicroservice.test2'];
const exchangeName = 'micro_exchange';
const queueName = 'rpc_queue';
//const host = 'localhost';
const host = 'messagebroker';

const opt = { credentials: require('amqplib').credentials.plain('admin', 'admin') }

amqp.connect(`amqp://${host}`, opt, function(error0, connection) {
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