const express = require('express');
const { MongoClient } = require('mongodb');

const app = express();

//Connection string
const uri = `mongodb://${process.env.MONGODB_ROOT_USERNAME}:${process.env.MONGODB_ROOT_PASSWORD}@${process.env.MONGO_SERVICE_NAME}:27017/${process.env.MONGODB_DATABASE}`;

let database;

const client = new MongoClient(uri);


// Middleware to connect to the database
const connectToDatabase = async () => {
    try {
        console.log("Trying to connect with Db");
        await client.connect();
        console.log("Connected to db");
        database = client.db();
    } catch (err) {
        console.error('Database connection error', err);
        throw err; 
    }
};

app.use(async (req, res, next) => {
    try {
        await connectToDatabase();
        next();
    } catch (err) {
        res.status(500).json({ error: 'Database connection error' });
    }
});


app.get('/check-db', async (req, res) => {
    try {
        console.log("check-db was hit");
        await client.db(process.env.MONGODB_DATABASE).command({ ping: 1 });
        res.json({ status: 'Connected to the database' });
    } catch (err) {
        console.error('Database connection error', err);
        res.status(500).json({ error: 'Database connection error' });
    }
});


app.get('/data', async (req, res) => {
    try {
        console.log("fetching data ...");
        const collection = database.collection('test');
        const result = await collection.find({}).toArray();
        res.json(result);
    } catch (err) {
        console.error('Error executing query', err);
        res.status(500).json({ error: 'Internal server error' });
    }
});

app.get('/', (req, res) => {
    res.send('Hello from the my app 2.0');
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, async () => {
    try {
        await connectToDatabase();
        console.log(`Server is running on port with latest changes on ${PORT}`);
    } catch (err) {
        console.error('Database connection error', err);
    }
});
