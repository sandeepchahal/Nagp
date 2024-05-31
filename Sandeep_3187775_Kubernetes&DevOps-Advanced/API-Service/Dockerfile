# Use the official Node.js image as a base
FROM node:18-alpine

# Create and change to the app directory
WORKDIR /app

# Copy the entrypoint script and ensure it is executable
COPY docker-entrypoint.sh /usr/local/bin/
RUN chmod +x /usr/local/bin/docker-entrypoint.sh

# Copy application code
COPY . /app

# Install dependencies
RUN npm install

# Expose the port your app runs on
EXPOSE 3000

# Set the entrypoint
ENTRYPOINT ["/usr/local/bin/docker-entrypoint.sh"]

# Specify the command to run your app
CMD ["npm", "run","start"]
