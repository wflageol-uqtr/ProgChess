export default function ProgChessLoader() {
  return (
    <div className="absolute bg-[rgba(0,0,0,0.5)] inset-0 flex items-center justify-center z-50">
      <div className="flex gap-3 items-center">
        <div className="flex text-2xl font-bold gap-1">
          <div className="animate-bounce [animation-delay:0ms]">Prog</div>
          <div className="animate-bounce [animation-delay:200ms]">Chess</div>
        </div>
        <div className="w-3 h-3 bg-white rounded-full animate-bounce [animation-delay:400ms]"></div>
        <div className="w-3 h-3 bg-white rounded-full animate-bounce [animation-delay:600ms]"></div>
        <div className="w-3 h-3 bg-white rounded-full animate-bounce [animation-delay:800ms]"></div>
      </div>
    </div>
  );
}
