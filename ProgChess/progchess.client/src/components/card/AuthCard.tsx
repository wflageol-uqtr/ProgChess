function AuthCard({ children }: any) {
  return (
    <div className="w-full max-w-sm md:max-w-md p-4 bg-zinc-800 border border-gray-100 rounded-lg shadow-sm sm:p-6 md:p-8">
      <h5 className="text-xl font-semibold text-green-500 text-center mb-6">
        ProgChess
      </h5>
      {children}
    </div>
  );
}

export default AuthCard;
