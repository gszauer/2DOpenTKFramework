using System;
using OpenTK.Audio;
using NAudio.Wave;
using OpenTK.Audio.OpenAL;
using System.Collections.Generic;

namespace GameFramework {
    class SoundManager {
        private class SoundInstance {
            public int bufferHandle = -1;
            public int soundSource = -1;
            public string path = string.Empty;
            public int refCount = 0;
        }

        private static SoundManager instance = null;
        public static SoundManager Instance {
            get {
                if (instance == null) {
                    instance = new SoundManager();
                }
                return instance;
            }
        }

        private SoundManager() {

        }

        private bool isInitialized = false;
        private List<SoundInstance> managedSounds = null;
        private AudioContext context = null;

        private void Error(string error) {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = old;
        }

        private void Warning(string error) {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(error);
            Console.ForegroundColor = old;
        }

        public void Initialize(OpenTK.GameWindow window) {
            if (isInitialized) {
                Error("Trying to double initialize sound manager!");
            }
            context = new AudioContext();
            managedSounds = new List<SoundInstance>();
            isInitialized = true;
        }

        public void Shutdown() {
            if (!isInitialized) {
                Error("Trying to double shut down sound manager!");
            }
            for (int i = 0; i < managedSounds.Count; ++i) {
                if (managedSounds[i].refCount > 0) {
                    Warning("Sound reference is > 0: " + managedSounds[i].path);
                }
                StopSound(i);
                AL.DeleteBuffer(managedSounds[i].bufferHandle);
                AL.DeleteSource(managedSounds[i].soundSource);
                managedSounds[i] = null;
            }
            managedSounds.Clear();
            managedSounds = null;
            isInitialized = false;
        }

        private ALFormat GetSoundFormat(int channels, int bits) {
            if (!isInitialized) {
                Error("Trying to get sound format for uninitialized sound manager");
            }
            switch (channels) {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        public int LoadMp3(string path) {
            if (!isInitialized) {
                Error("Trying to Load Mp3 format for uninitialized sound manager");
            }
            for (int i = 0; i < managedSounds.Count; ++i) {
                if (managedSounds[i].path == path) {
                    managedSounds[i].refCount += 1;
                    return i;
                }
            }

            int result = -1;
            using (WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(path))) {
                result = LoadStream(waveStream);
                managedSounds[result].path = path;
            }
            return result;
        }

        public int LoadWav(string path) {
            if (!isInitialized) {
                Error("Trying to Load Wav format for uninitialized sound manager");
            }
            for (int i = 0; i < managedSounds.Count; ++i) {
                if (managedSounds[i].path == path) {
                    managedSounds[i].refCount += 1;
                    return i;
                }
            }

            int result = -1;
            using (WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(new WaveFileReader(path))) {
                result = LoadStream(waveStream);
                managedSounds[result].path = path;
            }
            return result;
        }

        private int LoadStream(WaveStream waveStream) {
            if (!isInitialized) {
                Error("Trying to Load Stream format for uninitialized sound manager");
            }

            int channels, bits_per_sample, sample_rate;
            byte[] sound_data = null;

            for (int i = 0; i < managedSounds.Count; ++i) {
                if (managedSounds[i].refCount <= 0) {
                    StopSound(managedSounds[i].bufferHandle);
                    AL.DeleteBuffer(managedSounds[i].bufferHandle);
                    managedSounds[i].bufferHandle = AL.GenBuffer();
                    managedSounds[i].refCount = 1;

                    sound_data = new byte[waveStream.Length];
                    waveStream.Read(sound_data, 0, (int)waveStream.Length);
                    channels = waveStream.WaveFormat.Channels;
                    bits_per_sample = waveStream.WaveFormat.BitsPerSample;
                    sample_rate = waveStream.WaveFormat.SampleRate;
                    AL.BufferData(managedSounds[i].bufferHandle, GetSoundFormat(channels, bits_per_sample), sound_data, sound_data.Length, sample_rate);
                    AL.Source(managedSounds[i].soundSource, ALSourcei.Buffer, managedSounds[i].bufferHandle);

                    return i;
                }
            }

            SoundInstance newSound = new SoundInstance();
            newSound.refCount = 1;
            newSound.bufferHandle = AL.GenBuffer();
            newSound.soundSource = AL.GenSource();

            sound_data = new byte[waveStream.Length];
            waveStream.Read(sound_data, 0, (int)waveStream.Length);
            channels = waveStream.WaveFormat.Channels;
            bits_per_sample = waveStream.WaveFormat.BitsPerSample;
            sample_rate = waveStream.WaveFormat.SampleRate;
            AL.BufferData(newSound.bufferHandle, GetSoundFormat(channels, bits_per_sample), sound_data, sound_data.Length, sample_rate);
            AL.Source(newSound.soundSource, ALSourcei.Buffer, newSound.bufferHandle);

            managedSounds.Add(newSound);
            return managedSounds.Count - 1;
        }

        public void UnloadSound(int soundId) {
            if (!isInitialized) {
                Error("Trying to Unload Sound format for uninitialized sound manager");
            }
            managedSounds[soundId].refCount -= 1;
            if (managedSounds[soundId].refCount < 0) {
                Error("Ref count of texture is less than 0: " + managedSounds[soundId].path);
            }
        }

        public void PlaySound(int soundId) {
            if (!isInitialized) {
                Error("Trying to Play Sound format for uninitialized sound manager");
            }
            AL.SourceRewind(managedSounds[soundId].soundSource);
            if (!IsPlaying(soundId)) {
                AL.SourcePlay(managedSounds[soundId].soundSource);
            }
        }

        public bool IsPlaying(int soundId) {
            if (!isInitialized) {
                Error("Trying to Check sound format for uninitialized sound manager");
            }
            int state = (int)ALSourceState.Initial;
            AL.GetSource(managedSounds[soundId].soundSource, ALGetSourcei.SourceState, out state);
            return (ALSourceState)state == ALSourceState.Playing;
        }

        public void StopSound(int soundId) {
            if (!isInitialized) {
                Error("Trying to stop sound format for uninitialized sound manager");
            }
            AL.SourceStop(managedSounds[soundId].soundSource);
        }
        
        public void SetVolume(int soundId, float volume) {
            if (!isInitialized) {
                Error("Trying to set volume format for uninitialized sound manager");
            }
            if (volume < 0.0f) {
                volume = 0.0f;
                Warning("Trying to set volume less than 0 for: " + managedSounds[soundId].path);
            }
            if (volume > 1.0f) {
                volume = 1.0f;
                Warning("Trying to set volume greater than 1 for: " + managedSounds[soundId].path);

            }
            AL.Source(managedSounds[soundId].soundSource, ALSourcef.Gain, volume);
        }

        public float GetVolume(int soundId) {
            if (!isInitialized) {
                Error("Trying to get volume format for uninitialized sound manager");
            }
            float volume = -1.0f;

            AL.GetSource(managedSounds[soundId].soundSource, ALSourcef.Gain, out volume);

            return volume;
        }
    }
}