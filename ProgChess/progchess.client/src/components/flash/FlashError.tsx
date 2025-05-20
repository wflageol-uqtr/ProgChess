interface FlashErrorProps {
  message: string;
}

function FlashError({ message }: FlashErrorProps) {
  return (
    <div className="p-4 mb-4 text-sm text-red-800 rounded-lg bg-red-50">
      <span className="font-medium">Erreur !</span> {message}
    </div>
  );
}

export default FlashError;
