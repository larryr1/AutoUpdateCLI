import { Socket } from "socket.io";

export default function onConnection(socket: Socket) {
  console.log(`New connection from ${socket.handshake.address}.`);
  
  // Get socket query data
  const query = socket.handshake.query;
  
  // Error if missing required parameters
  if (!query["machineId"]) {
    disconnectWithError(socket, "Socket connection query is missing parameter machineId.");
    return;
  }

  if (!query["os"]) {
    disconnectWithError(socket, "Socket connection query is missing parameter os.");
  }

  if (!query["machineName"]) {
    disconnectWithError(socket, "Socket connection query is missing parameter machineName.");
  }
}

function disconnectWithError(socket: Socket, reason: String) {
  console.log(`Disconnecitng socket ${socket.id} with reason: ${reason}`);
  socket.emit("disconnectReason", { success: false, message: reason });
  socket.disconnect();
}